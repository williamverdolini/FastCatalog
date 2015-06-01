using System.Collections.Generic;
using SQL2Elastic.Models;
using SQLCommon.Models;

namespace SQL2Elastic.Logic
{
    public static class SQLExtensions
    {
        public static List<ProductAttribute> ToProductAttributes(this List<SQLAttribute> attributes)
        {
            List<ProductAttribute> attrs = new List<ProductAttribute>();
            foreach (var a in attributes)
            {
                attrs.Add(new ProductAttribute
                {
                    Key = a.Key,
                    Value = a.Value
                });
            }
            return attrs;
        }
    }
}
