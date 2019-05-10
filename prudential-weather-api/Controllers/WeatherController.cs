using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using prudential_weather_api.business;
using prudential_weather_api.Contract;
using model = prudential_weather_api.business.Model;

namespace prudential_weather_api.Controllers
{
    public class WeatherController : ApiController
    {

        private readonly IWeatherMiddleware weatherMiddleware;
        private readonly IMapper mapper;

        public WeatherController(IWeatherMiddleware weatherMiddleware,IMapper mapper)
        {
            this.weatherMiddleware = weatherMiddleware;
            this.mapper = mapper;
        }


        public async Task<IHttpActionResult> Post(IEnumerable<City> cities)
        {
            var response = await weatherMiddleware.GetWeather(mapper.Map<IEnumerable<model.City>>(cities));
            return Ok(response);
        }
    }
}
