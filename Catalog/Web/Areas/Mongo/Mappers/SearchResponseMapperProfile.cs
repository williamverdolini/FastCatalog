using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using Web.Models;
using Web.Models.Search;

namespace Web.Areas.Mongo.Mappers
{
    public class SearchResponseMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<BsonDocument, ProductAttributeAggregation>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src["_id"].AsString))
                .ForMember(dest => dest.Values, opt => opt.ResolveUsing<ProductAttributesResolver>());

            CreateMap<BsonDocument, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => AsGuid(src["_id"].AsObjectId)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src["Code"].AsString))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src["Description"].AsString))
                .ForMember(dest => dest.IdCategory, opt => opt.MapFrom(src => src["IdCategory"].AsInt64))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src["Price"].AsDouble))
                .ForMember(dest => dest.Synonims, opt => opt.MapFrom(src => new List<string>()))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => new List<ProductAttribute>()))
                ;
        }

        public Guid AsGuid(ObjectId oid)
        {
            var bytes = oid.ToByteArray().Concat(new byte[] { 5, 5, 5, 5 }).ToArray();
            Guid gid = new Guid(bytes);
            return gid;
        }


        public class ProductAttributesResolver : ValueResolver<BsonDocument, IList<ValueCount>>
        {
            protected override IList<ValueCount> ResolveCore(BsonDocument source)
            {
                var attributes = new List<ValueCount>();
                var dbAttributes = source["Properties"].AsBsonArray;

                foreach (var val in dbAttributes)
                {                    
                    attributes.Add(new ValueCount
                    {
                        Value = val.AsBsonDocument["Value"].AsString,
                        Count = val.AsBsonDocument["Count"].AsInt32
                    });
                }
                return attributes;
            }
        }
    }
}