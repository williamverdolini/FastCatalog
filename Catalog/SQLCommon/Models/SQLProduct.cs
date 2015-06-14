using System.Collections.Generic;
using System.Xml.Serialization;

namespace SQLCommon.Models
{
    [XmlRoot("Product")]
    public class SQLProduct
    {
        public SQLData Data { get; set; }
        [XmlElement("Attribute")]
        public List<SQLAttribute> Attributes { get; set; }
        [XmlElement("Synonim")]
        public List<SQLSynonim> Synonims { get; set; }
    }

    public class SQLData
    {
        public long IDProdotto { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long IdCategory { get; set; }
    }

    public class SQLAttributes
    {
        [XmlElement("Attribute")]
        public List<SQLAttribute> Attributes { get; set; }
    }

    public class SQLAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SQLSynonim
    {
        [XmlAttribute("Code")]
        public string Code { get; set; }
    }
}
