using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using prudential_weather_api.business;
using prudential_weather_api.Contract;
using model = prudential_weather_api.business.Model;

namespace prudential_weather_api.Controllers
{
    /// <summary>
    /// Weather controller contains endpoints related to Weather
    /// </summary>
    public class WeatherController : ApiController
    {
        /// <summary>
        /// Instance of <see cref="weatherMiddleware"/>
        /// </summary>
        private readonly IWeatherMiddleware weatherMiddleware;

        /// <summary>
        /// Instance of <see cref="Mapper"/>
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor holding dependencies of controller
        /// </summary>
        /// <param name="weatherMiddleware">Instance of <see cref="weatherMiddleware"/></param>
        /// <param name="mapper">Instance of <see cref="Mapper"/></param>
        public WeatherController(IWeatherMiddleware weatherMiddleware,IMapper mapper)
        {
            this.weatherMiddleware = weatherMiddleware;
            this.mapper = mapper;
        }

        /// <summary>
        /// Web API Endpoint used to retrieve Weather information based on City Id 
        /// </summary>
        /// <param name="cities">List of Cities with ID</param>
        /// <returns>Cities Weather information</returns>
        public async Task<IHttpActionResult> Post(IEnumerable<City> cities)
        {
            if (cities == null || !cities.Any()) return BadRequest("No data provided");

            var request = mapper.Map<IEnumerable<model.City>>(cities);
            var response = await weatherMiddleware.GetWeather(request);
            return response == null ? InternalServerError() : (IHttpActionResult)Ok(response);
        }
    }
}
