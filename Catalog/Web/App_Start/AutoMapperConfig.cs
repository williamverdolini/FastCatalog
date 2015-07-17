﻿using System;
using System.Linq;
using AutoMapper;
using Web.Areas.Elastic.Mappers;

namespace Web
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
        }

        private static void GetConfiguration(IConfiguration configuration)
        {
            var profiles = typeof(SearchInputMapperProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            foreach (var profile in profiles)
            {
                configuration.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }
    }
}