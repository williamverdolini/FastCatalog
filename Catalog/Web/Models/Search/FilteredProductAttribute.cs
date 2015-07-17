using System.Collections.Generic;

namespace Web.Models.Search
{
    public class FilteredProductAttribute
    {
        public string Key { get; set; }
        public IList<string> Values { get; set; }
    }
}