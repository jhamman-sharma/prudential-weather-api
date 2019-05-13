using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Linq;
using prudential_weather_api.business.Model;
using prudential_weather_api.common;
using prudential_weather_api.repository;
using Entities = prudential_weather_api.repository.Entities;

namespace prudential_weather_api.business
{
    public interface IWeatherMiddleware
    {
        Task<IList<OpenWeather>> GetWeather(IEnumerable<City> cities);
    }

    public class WeatherMiddleware : IWeatherMiddleware
    { 
        private readonly IOpenWeatherServiceRepository openWeatherServiceRepository;
        private readonly IMapper mapper;

        public WeatherMiddleware(IOpenWeatherServiceRepository openWeatherServiceRepository,IMapper mapper)
        {
            this.openWeatherServiceRepository = openWeatherServiceRepository;
            this.mapper = mapper;
        }

        public async Task<IList<OpenWeather>> GetWeather(IEnumerable<City> cities)
        {
            var citiesWeather = new List<Task<Entities.OpenWeather>>();
            foreach (var city in cities)
            {
                var response = openWeatherServiceRepository.GetWeather(city.Id);          
                citiesWeather.Add(response);
            }
            var finalResponse  = await Task.WhenAll(citiesWeather);
            var result = mapper.Map<IList<OpenWeather>>(finalResponse);
            SaveResult(result);
            return result;
        }

        private void SaveResult(IList<OpenWeather> weathers)
        {
            foreach (var city in weathers)
            {
                var json = (JObject)JToken.FromObject(city);
                var filename = $"{city.name}";
                FileStorage.StoreJSONFile(json, filename+"_"+DateTime.Now.ToString("yyyyMMdd"));
            }
        }


    }
}

