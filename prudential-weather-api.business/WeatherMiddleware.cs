using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Linq;
using prudential_weather_api.business.Model;
using prudential_weather_api.common;
using prudential_weather_api.repository;
using Entities = prudential_weather_api.repository.Entities;

namespace prudential_weather_api.business
{
    /// <summary>
    /// Interface for <see cref="WeatherMiddleware"/>
    /// </summary>
    public interface IWeatherMiddleware
    {
        /// <summary>
        /// Method used to retrieve Weather Information from Open Weather 3rd Party Service and also it also log data on specified path
        /// </summary>
        /// <param name="cities">List of cities with unique ID</param>
        /// <returns>Weather Information of cities by ID</returns>
        Task<IList<OpenWeather>> GetWeather(IEnumerable<City> cities);
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeatherMiddleware : IWeatherMiddleware
    { 
        /// <summary>
        /// Instance of <see cref="OpenWeatherServiceRepository"/>
        /// </summary>
        private readonly IOpenWeatherServiceRepository openWeatherServiceRepository;

        /// <summary>
        /// Instance of <see cref="Mapper"/>
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor used to initialize dependencies 
        /// </summary>
        /// <param name="openWeatherServiceRepository">Instance of <see cref="OpenWeatherServiceRepository"/></param>
        /// <param name="mapper">Instance of <see cref="Mapper"/></param>
        public WeatherMiddleware(IOpenWeatherServiceRepository openWeatherServiceRepository,IMapper mapper)
        {
            this.openWeatherServiceRepository = openWeatherServiceRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Method used to Process and log Weather information
        /// </summary>
        /// <param name="cities">Cities with ID</param>
        /// <returns>List Weather information by Id</returns>
        public async Task<IList<OpenWeather>> GetWeather(IEnumerable<City> cities)
        {
            var citiesWeather = new List<Task<Entities.OpenWeather>>();
            foreach (var city in cities)
            {
                var response = openWeatherServiceRepository.GetWeather(city.Id);
                citiesWeather.Add(response);
            }

            Entities.OpenWeather[] finalResponse = await Task.WhenAll(citiesWeather);
            var result = ProcessResponse(finalResponse);
            return result;
        }

        /// <summary>
        /// Method mapped response from service to Model 
        /// </summary>
        /// <param name="finalResponse">Response received from service</param>
        /// <returns>List Weather information by Id</returns>
        private IList<OpenWeather> ProcessResponse(Entities.OpenWeather[] finalResponse)
        {
            IList<OpenWeather> result = null;
            if (finalResponse.Any(r => r != null))
            {
                result = mapper.Map<IList<OpenWeather>>(finalResponse);
                LogResponse(result);
            }
            return result;
        }

        /// <summary>
        /// Method used to log response from service by date in JSON format at specified location
        /// </summary>
        /// <param name="weathers">Weather information which needs to log</param>
        private void LogResponse(IList<OpenWeather> weathers)
        {            
            foreach (var city in weathers)
            {
                var json = (JObject)JToken.FromObject(city);
                var filename = $"{city.name}";
                FileStorage.StoreJSONFile(json, filename + "_" + DateTime.Now.ToString("yyyyMMdd"));
            }
        }


    }
}

