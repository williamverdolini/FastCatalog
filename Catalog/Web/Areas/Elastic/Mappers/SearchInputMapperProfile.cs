using System.Collections.Generic;
using AutoMapper;
using Web.Models.Search;
using Web.Models.Views;
using System.Linq;

namespace Web.Areas.Elastic.Mappers
{
    public class SearchInputMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SearchInputViewModel, SearchInput>();
            CreateMap<FilteredProductAttributeViewModel, FilteredProductAttribute>();
        }
    }

    public class OrderedSearchInputMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<OrderedSearchInputViewModel, OrderedSearchInput>();
            CreateMap<OrderedProductAttributeViewModel, OrderedProductAttribute>();
            CreateMap<OrderedSearchInput, SearchInput>()
                .ForMember(dest => dest.Attributes, opt => opt.ResolveUsing<AttributesResolver>());
            // Mapping for easily clone the objects
            CreateMap<OrderedSearchInput, OrderedSearchInput>();
            CreateMap<SearchInput, SearchInput>();
        }
    }

    public class AttributesResolver : ValueResolver<OrderedSearchInput, IList<FilteredProductAttribute>>
    {
        protected override IList<FilteredProductAttribute> ResolveCore(OrderedSearchInput source)
        {
            var attributes = new List<FilteredProductAttribute>();
            foreach (string key in source.Attributes.Select(o => o.Key).Distinct())
            {
                attributes.Add(new FilteredProductAttribute { 
                    Key = key,
                    Values = source.Attributes.Where(s => s.Key.Equals(key)).Select(o => o.Value).ToList()
                });
            }
            return attributes;
        }
    }
}