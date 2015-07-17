using AutoMapper;
using Web.Models.Search;
using Web.Models.Views;

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
}