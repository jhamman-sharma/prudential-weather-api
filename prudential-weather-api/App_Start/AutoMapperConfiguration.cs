using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Model = prudential_weather_api.business.Model;
using Entities = prudential_weather_api.repository.Entities;
using Contract = prudential_weather_api.Contract;

namespace prudential_weather_api
{
    public static class AutoMapperConfiguration
    {
        public static IMapper Configuration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Contract.City, Model.City>();
                cfg.CreateMap<Entities.OpenWeather, Model.OpenWeather>();
            });
            return config.CreateMapper();
        }
    }
}