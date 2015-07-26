using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Search
{
    public class SearchInput
    {
        public int ValuesMaxNum { get; set; }
        public IList<FilteredProductAttribute> Attributes { get; set; }
        public int Size { get; set; }
        public int From { get; set; }
    }

    public class OrderedSearchInput
    {
        public int ValuesMaxNum { get; set; }
        public IList<OrderedProductAttribute> Attributes { get; set; }
        public int Size { get; set; }
        public int From { get; set; }
    }
}