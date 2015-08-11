using System;
using System.Collections.Generic;
using AutoMapper;
using Nest;
using Web.Models;
using Web.Models.Search;

namespace Web.Areas.Elastic.Mappers
{
    public class SearchResponseMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ISearchResponse<Product>, IList<ProductAttributeAggregation>>()
                .ConvertUsing(mappingFunction);
        }

        Func<ISearchResponse<Product>, IList<ProductAttributeAggregation>> mappingFunction = (source) =>
        {
            var bucket = source.Aggs.Children("multi_properties");

            IList<ProductAttributeAggregation> aggs = new List<ProductAttributeAggregation>();
            foreach (var item in bucket.Terms("all_properties").Items)
            {
                IList<ValueCount> values = new List<ValueCount>();

                foreach (var val in item.Terms("all_values_per_property").Items)
                {
                    values.Add(new ValueCount
                    {
                        Value = val.Key,
                        Count = val.DocCount
                    });
                }

                aggs.Add(new ProductAttributeAggregation
                {
                    Key = item.Key,
                    Values = values
                });
            }

            return aggs;
        };
    }
}