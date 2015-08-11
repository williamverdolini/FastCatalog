using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Web.Areas.Mongo.Models;
using Web.Infrastructure.Common;
using Web.Models;
using Web.Models.Search;

namespace Web.Areas.Mongo.Services
{
    public class CatalogRepository : ICatalogRepository
    {
        private const string MONGO_DB = "Catalog";
        private const string MONGO_COLLECTION = "Products";

        private readonly IMongoClient client;
        private readonly IMongoDatabase database;
        private readonly IMappingEngine mapper;

        public CatalogRepository(IMappingEngine mapper)
        {
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            this.mapper = mapper;
            client = new MongoClient();
            database = client.GetDatabase(MONGO_DB);            
        }

        public async Task<SearchResult> Search(SearchInput input)
        {
            SearchResult result;
            if (input != null && input.Attributes.Count > 0)
            {
                SearchInput clonedInput = mapper.Map<SearchInput, SearchInput>(input);
                FilteredProductAttribute lastAttribute = clonedInput.Attributes.Last();
                clonedInput.Attributes.Remove(lastAttribute);

                SearchResult first = await AggregationsSearch(clonedInput);
                var previousAggregation = first.Aggregations.First(a => a.Key.Equals(lastAttribute.Key));

                result = await CompleteSearch(input);
                var toRemove = result.Aggregations.First(a => a.Key.Equals(lastAttribute.Key));
                result.Aggregations.Remove(toRemove);
                result.Aggregations.Add(previousAggregation);
            }
            else
            {
                result = await CompleteSearch(input);
            }
            result.Aggregations = result.Aggregations.OrderBy(a => a.Key).ToList();

            return result;
        }

        #region Result Composition
        private async Task<SearchResult> CompleteSearch(SearchInput input)
        {
            SearchResult query = await SearchDocuments(input);

            return new SearchResult
            {
                Aggregations = mapper.Map<IList<BsonDocument>, IList<ProductAttributeAggregation>>(await SearchAggregations(input)),
                Results = query.Results,
                Count = query.Count
            };
        }
        private async Task<SearchResult> AggregationsSearch(SearchInput input)
        {
            return new SearchResult
            {
                Aggregations = mapper.Map<IList<BsonDocument>, IList<ProductAttributeAggregation>>(await SearchAggregations(input)),
                Results = null,
                Count = 0
            };
        }
        #endregion

        #region Mongo API method
        private BsonDocument MatchDocuments(SearchInput input)
        {
            List<BsonDocument> criteria = new List<BsonDocument>();

            if (input != null)
            {
                foreach (var attribute in input.Attributes)
                {
                    criteria.Add(new BsonDocument("$and",
                        new BsonArray { 
                    new BsonDocument{ {"Attributes.Key", attribute.Key} }, 
                    new BsonDocument{ { "Attributes.Value", new BsonDocument{{"$in",new BsonArray(attribute.Values.AsEnumerable())}}}}}));
                }
            }

            var filter = criteria.Count.Equals(0) ? new BsonDocument() : new BsonDocument{
                        {
                            "$and", new BsonArray(criteria.AsEnumerable())
                        }
                    };
            return filter;
        }
        //private async Task<SearchResult> SearchDocuments(SearchInput input)
        //{
        //    var collection = database.GetCollection<BsonDocument>(MONGO_COLLECTION);
        //    var cursor = collection.Find(MatchDocuments(input));
        //    long count = await cursor.CountAsync();
        //    var result = await cursor.Skip(0).Limit(10).ToListAsync();

        //    return new SearchResult 
        //    {
        //        Count = count,
        //        Results = mapper.Map<IList<BsonDocument>, IList<Product>>(result)
        //    };
        //}
        private async Task<SearchResult> SearchDocuments(SearchInput input)
        {
            var collection = database.GetCollection<MongoProduct>(MONGO_COLLECTION);
            var cursor = collection.Find(MatchDocuments(input));
            long count = await cursor.CountAsync();
            var result = await cursor.Skip(0).Limit(10).ToListAsync();

            return new SearchResult
            {
                Count = count,
                Results = mapper.Map<IList<MongoProduct>, IList<Product>>(result)
            };
        }
        private async Task<IList<BsonDocument>> SearchAggregations(SearchInput input)
        {
            var collection = database.GetCollection<BsonDocument>(MONGO_COLLECTION);

            var match = new BsonDocument{
                {
                    "$match", MatchDocuments(input)
                }
            };

            var unwindAttributes = new BsonDocument{
                {
                    "$unwind", "$Attributes"
                }
            };

            var groupAttributes = new BsonDocument{
                {
                    "$group", new BsonDocument{
                        {"_id", "$Attributes"}, 
                        {"total", new BsonDocument("$sum", 1)} 
                    }
                }
            };

            var sort = new BsonDocument{
                {
                    "$sort", new BsonDocument{
                        {"_id.Value", 1}
                    }
                }
            };

            var groupValuesPerAttribute = new BsonDocument{
                {
                    "$group", new BsonDocument{
                        {"_id", "$_id.Key"}, 
                        {"Properties", new BsonDocument{
                            {
                                "$push", new BsonDocument{
                                        {"Value", "$_id.Value"},
                                        {"Count", "$total"}
                                    }
                                }
                            } 
                        }
                    }
                }
            };

            var sortValues = new BsonDocument{
                {
                    "$sort", new BsonDocument{
                        {"_id", 1},
                        {"Properties.Value", 1}
                    }
                }
            };

            var aggregate = await collection.AggregateAsync<BsonDocument>(new[] { 
                match,
                unwindAttributes, 
                groupAttributes, 
                sort, 
                groupValuesPerAttribute, 
                sortValues 
            });

            return aggregate.ToListAsync().Result;
        }
        #endregion

    }
}