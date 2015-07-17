using System;
using System.Collections.Generic;
using Nest;

namespace Web.Areas.Elastic
{    
    [ElasticType]
    public class ESProduct
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long IdCategory { get; set; }
        public IList<string> Synonims { get; set; }
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


    //public class FilteredProductAttribute
    //{
    //    public string Key { get; set; }
    //    public IList<string> Values { get; set; }
    //}

    //public class ProductAttributeAggregation
    //{
    //    public string Key { get; set; }
    //    public string Value { get; set; }
    //    public int Count { get; set; }
    //}

    //public class SearchResult<TAggregation, TRecord>
    //{
    //    IList<TAggregation> Aggregations { get; set; }
    //    IList<TRecord> Results { get; set; }
    //}

}
