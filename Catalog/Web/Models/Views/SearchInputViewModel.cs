using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Views
{
    public class SearchInputViewModel
    {
        //public int ValuesMaxNum { get; set; }
        public IList<FilteredProductAttributeViewModel> Attributes { get; set; }
        //public int Size { get; set; }
        //public int From { get; set; }
    }

    public class FilteredProductAttributeViewModel
    {
        public string Key { get; set; }
        public IList<string> Values { get; set; }
    }

    public class OrderedSearchInputViewModel
    {
        //public int ValuesMaxNum { get; set; }
        public IList<OrderedProductAttributeViewModel> Attributes { get; set; }
        //public int Size { get; set; }
        //public int From { get; set; }
    }

    public class OrderedProductAttributeViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}