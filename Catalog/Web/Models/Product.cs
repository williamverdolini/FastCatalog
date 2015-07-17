using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long IdCategory { get; set; }
        public IList<string> Synonims { get; set; }
        public IList<ProductAttribute> Attributes { get; set; }
    }

    public class ProductAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}