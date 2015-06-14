using System;
using System.Collections.Generic;
using Nest;

namespace SQL2Elastic.Models
{    
    [ElasticType]
    public class ESProduct
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Type = FieldType.String)]
        public Guid Id { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Code { get; set; }
        public string Description { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public double Price { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public long IdCategory { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public IList<string> Synonims { get; set; }
        [ElasticProperty(Type = FieldType.Nested)]
        public IList<ProductAttribute> Attributes { get; set; }
    }

    [ElasticType]
    public class ProductAttribute
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Key { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Value { get; set; }
    }
}
