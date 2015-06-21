using System;
using System.Collections.Generic;
using Nest;
using SQL2Elastic.Models;
using SQLCommon.Logic;
using SQLCommon.Models;

namespace SQL2Elastic.Logic
{
    public class ElasticSearchClient : IDbClient
    {
        private ElasticClient client;
        private IList<ESProduct> products;
        private Random rnd;

        public IDbClient Initialize()
        {
            rnd = new Random();
            // Elastich Search Settings
            var settings = new ConnectionSettings(
                                    uri: new Uri(Resources.ES_NodeUri),
                                    defaultIndex: Resources.ES_defaultIndex);

            // Elastich Search Client
            client = new ElasticClient(settings);
            products = new List<ESProduct>();
            CreateMapping();

            return this;
        }

        public void Save(SQLProduct dbProduct)
        {
            Contract.Requires<ArgumentNullException>(dbProduct != null, "dbProduct");
            var product = new ESProduct
            {
                Id = Guid.NewGuid(),
                Code = dbProduct.Data.Code,
                Description = dbProduct.Data.Description,
                IdCategory = dbProduct.Data.IdCategory,
                Price = Math.Round(10 + rnd.NextDouble() * (1000 - 10), 2),
                Synonims = dbProduct.Synonims.ToStringList(),
                Attributes = dbProduct.Attributes.ToProductAttributes()
            };
            products.Add(product);
        }

        public void FlushProducts(int commitStep = 0)
        {
            if (products.Count > 0 && (products.Count.Equals(commitStep) || commitStep.Equals(0)))
            {
                client.Bulk(b => b.IndexMany<ESProduct>(products,
                             (bulkDescriptor, record) => bulkDescriptor.Type(Resources.ES_Type)));
                products.Clear();
            }
        }

        private void CreateMapping()
        {
            var result = client.IndexExists(i => i.Index(Resources.ES_defaultIndex));
            if (!result.Exists)
            {
                // Do Mapping before to migrate (not during indexing documents)
                // see: https://github.com/elastic/elasticsearch-net/issues/1242
                client.CreateIndex(Resources.ES_defaultIndex, c => c
                    .AddMapping<ESProduct>(m => m
                        .IgnoreConflicts()
                        .MapFromAttributes()
                        .Type(Resources.ES_Type)
                        )
                    );
            }
        }


        public void PostMigration()
        {
            //No post-migration task needed
        }
    }
}
