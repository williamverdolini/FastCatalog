using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SQL2MongoDB.Models;
using SQLCommon.Logic;
using SQLCommon.Models;

namespace SQL2MongoDB.Logic
{
    public class MongoDbClient : IDbClient
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IList<BsonDocument> products;
        private Random rnd;

        public IDbClient Initialize()
        {
            rnd = new Random();
            client = new MongoClient();
            products = new List<BsonDocument>();
            database = client.GetDatabase(Resources.MongoDB);
            return this;
        }

        public void Save(SQLProduct dbProduct)
        {
            Contract.Requires<ArgumentNullException>(dbProduct != null, "dbProduct");
            var product = new MongoProduct
            {
                //Id = Guid.NewGuid().ToString(),
                Code = dbProduct.Data.Code,
                Description = dbProduct.Data.Description,
                IdCategory = dbProduct.Data.IdCategory,
                Price = Math.Round(10 + rnd.NextDouble() * (1000 - 10),2),
                Synonims = dbProduct.Synonims.ToStringList(),
                Attributes = dbProduct.Attributes.ToProductAttributes()
            };
            products.Add(product.ToBsonDocument());
        }

        public void FlushProducts(int commitStep = 0)
        {
            if (products.Count > 0 && (products.Count.Equals(commitStep) || commitStep.Equals(0)))
            {
                var collection = database.GetCollection<BsonDocument>(Resources.MongoCollection);
                collection.InsertManyAsync(products.ToList());
                products.Clear();
            }
        }

        public void PostMigration()
        {
            // Create Indices to speed search
            Console.WriteLine("Start Indexing...");
            var task = CreateFirstIndex();
            task.Wait();

            task = CreateSecondIndex();
            task.Wait();
            Console.WriteLine("Indexing Complete.");
        }

        private async Task CreateFirstIndex()
        {
            var collection = database.GetCollection<BsonDocument>(Resources.MongoCollection);
            var options = new CreateIndexOptions
            {
                Background = true,
            };
            await collection.Indexes.CreateOneAsync(Builders<BsonDocument>.IndexKeys.Ascending("Attributes.Value"), options);
        }

        private async Task CreateSecondIndex()
        {
            var collection = database.GetCollection<BsonDocument>(Resources.MongoCollection);
            var options = new CreateIndexOptions
            {
                Background = true,
            };
            await collection.Indexes.CreateOneAsync(Builders<BsonDocument>.IndexKeys.Combine(
                Builders<BsonDocument>.IndexKeys.Ascending("_id"),
                Builders<BsonDocument>.IndexKeys.Ascending("Attributes.Key"),
                Builders<BsonDocument>.IndexKeys.Ascending("Attributes.Value")
                ), options);
        }
    }

}
