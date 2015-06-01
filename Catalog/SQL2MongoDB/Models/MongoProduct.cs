using System.Collections.Generic;

namespace SQL2MongoDB.Models
{

    public class MongoProduct
    {
        //public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IList<string> Synonims { get; set; }
        public IList<ProductAttribute> Attributes { get; set; }
    }

    public class ProductAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
        //public IList<string> Values { get; set; }
    }

}
