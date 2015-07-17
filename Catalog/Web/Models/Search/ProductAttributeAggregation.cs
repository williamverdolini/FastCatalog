
using System.Collections.Generic;
namespace Web.Models.Search
{
    public class ProductAttributeAggregation
    {
        public string Key { get; set; }
        public IList<ValueCount> Values { get; set; }
    }

    public class ValueCount
    {
        public string Value { get; set; }
        public long Count { get; set; }
    }
}