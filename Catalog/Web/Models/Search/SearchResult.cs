using System.Collections.Generic;

namespace Web.Models.Search
{
    public class SearchResult
    {
        public IList<ProductAttributeAggregation> Aggregations { get; set; }
        public IList<Product> Results { get; set; }
        public long Count { get; set; }
    }
}