using prudential_weather_api.common;
using prudential_weather_api.repository.Entities;
using System.Threading.Tasks;

namespace prudential_weather_api.repository
{
    public interface IOpenWeatherServiceRepository
    {
        Task<OpenWeather> GetWeather(int id);
    }

    public class OpenWeatherServiceRepository : Service<OpenWeather>, IOpenWeatherServiceRepository
    {
        private readonly string appId;

        public OpenWeatherServiceRepository(string baseUrl,string appId) : base(baseUrl)
        {
            this.appId = appId;
        }
        
        public async Task<OpenWeather> GetWeather(int id)
        {
            return await GetAsync($"weather?id={id}&appid={appId}");
        }
    }
}
