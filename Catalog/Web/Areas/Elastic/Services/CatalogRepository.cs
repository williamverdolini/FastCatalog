using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Web.Models;
using Web.Models.Search;

namespace Web.Areas.Elastic.Services
{

    public class CatalogRepository : ICatalogRepository
    {
        private const string ELASTIC_HOST = "http://localhost:9200";
        private const string DEFAULT_INDEX = "catalog";
        private const string PRODUCT_TYPE_NAME = "products";
        private const string PATH_ATTRIBUTES = "attributes";
        private const string MULTI_PROPERTIES_QUERY = "multi_properties";
        private const string ALL_PROPERTIES_AGG = "all_properties";
        private const string ALL_VALUES_PER_PROPERTY = "all_values_per_property";

        private ElasticClient client;

        public CatalogRepository()
        {
            client = new ElasticClient(new ConnectionSettings(
                            uri: new Uri(ELASTIC_HOST),
                            defaultIndex: DEFAULT_INDEX)
                                .MapDefaultTypeNames(
                                    t => t.Add(typeof(Product), PRODUCT_TYPE_NAME)
                                ));
        }

        public async Task<SearchResult> Search(SearchInput input)
        {
            var result = await client.SearchAsync<Product>(s => s
                .Aggregations(a => a
                    .Nested(MULTI_PROPERTIES_QUERY, n => n
                        .Path(PATH_ATTRIBUTES)
                        .Aggregations(na => na
                            .Terms(ALL_PROPERTIES_AGG, f => f
                                .Field(o => o.Attributes.First().Key)
                                .Size(0)
                                .Aggregations(nna => nna
                                    .Terms(ALL_VALUES_PER_PROPERTY, nf => nf
                                        .Field(o => o.Attributes.First().Value)
                                        .Size(5)))))
                        )
                    )
                .Query(q => q.MatchAll() && GetInputQueryContainer(input))
                );

            return MapToSearchResult(result);
        }


        #region Query Composer
        private QueryContainer GetInputQueryContainer(SearchInput input)
        {
            IList<FilterContainer> filters = new List<FilterContainer> { };
            foreach (var attribute in input.Attributes)
            {
                filters.Add(new FilterDescriptor<Product>().Nested(n => n.Path(PATH_ATTRIBUTES)
                    .Query(q => q
                        .Bool(qq => qq
                            .Must(iq =>
                            {
                                QueryContainer query = null;
                                query &= q.Term(t => t.Attributes.First().Key, attribute.Key);
                                query &= q.Terms(t => t.Attributes.First().Value, attribute.Values);
                                return query;
                            })
                             ))));
            }

            var termQuery = Query<Product>
                .Filtered(f => f
                    .Filter(ff => ff
                        .Bool(fff => fff
                            .Must(filters.ToArray())))
                );
            return termQuery;
        }
        #endregion

        #region Mappings (To-DO: usare AutoMapper, o ExpressMapper)
        private IList<ProductAttributeAggregation> MapToProductAttributeAggregation(ISearchResponse<Product> elasticResponse)
        {
            var bucket = elasticResponse.Aggs.Children(MULTI_PROPERTIES_QUERY);

            IList<ProductAttributeAggregation> aggs = new List<ProductAttributeAggregation>();
            foreach (var item in bucket.Terms(ALL_PROPERTIES_AGG).Items)
            {
                IList<ValueCount> values = new List<ValueCount>();

                foreach (var val in item.Terms(ALL_VALUES_PER_PROPERTY).Items)
                {
                    values.Add(new ValueCount
                    {
                        Value = val.Key,
                        Count = val.DocCount
                    });
                }

                aggs.Add(new ProductAttributeAggregation
                {
                    Key = item.Key,
                    Values = values
                });
            }

            return aggs;
        }
        private SearchResult MapToSearchResult(ISearchResponse<Product> elasticResponse)
        {
            return new SearchResult
            {
                Aggregations = MapToProductAttributeAggregation(elasticResponse),
                Results = elasticResponse.Documents.ToList(),
                Count = elasticResponse.Total
            };
        }
        #endregion
    }
}