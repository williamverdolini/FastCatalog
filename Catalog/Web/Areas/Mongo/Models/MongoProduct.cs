using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.Areas.Mongo.Models
{
    public class MongoProduct
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        [BsonElement("Code")]
        public string Code { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Price")]
        public double Price { get; set; }
        [BsonElement("IdCategory")]
        public long IdCategory { get; set; }
        public IList<string> Synonims { get; set; }
        public IList<MongoProductAttribute> Attributes { get; set; }
    }

    public class MongoProductAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}