using System.Threading.Tasks;
using prudential_weather_api.common;
using prudential_weather_api.repository.Entities;

namespace prudential_weather_api.repository
{
    /// <summary>
    /// Instance of <see cref="OpenWeatherServiceRepository"/>
    /// </summary>
    public interface IOpenWeatherServiceRepository
    {
        /// <summary>
        /// Method call Open Weather REST service and retrieve Weather information by city ID
        /// </summary>
        /// <param name="id">City Id</param>
        /// <returns>Weather Information</returns>
        Task<OpenWeather> GetWeather(int id);
    }

    /// <summary>
    /// Repository used to talk with Open Weather REST service
    /// </summary>
    public class OpenWeatherServiceRepository : Service<OpenWeather>, IOpenWeatherServiceRepository
    {
        /// <summary>
        ///  Open Weather REST service APP ID
        /// </summary>
        private readonly string appId;

        /// <summary>
        /// Constructor of <see cref="OpenWeatherServiceRepository"/>
        /// </summary>
        /// <param name="baseUrl">Open Weather REST service base URL</param>
        /// <param name="appId">Open Weather REST service APP ID</param>
        public OpenWeatherServiceRepository(string baseUrl,string appId) : base(baseUrl)
        {
            this.appId = appId;
        }

        /// <summary>
        /// Method call Open Weather REST service and retrieve Weather Information by city ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OpenWeather> GetWeather(int id)
        {
            return await GetAsync($"weather?id={id}&appid={appId}");
        }
    }
}
