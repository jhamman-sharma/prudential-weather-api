using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace prudential_weather_api.common
{
    public interface IService<T>
    {
        Task<T> GetAsync(string urlSegment);
    }

    public abstract class Service<T> : IService<T>
    {
        private readonly string baseUrl;
        private readonly string mediaType;

        public Service(string baseUrl)
        {
            this.baseUrl = baseUrl;
            mediaType = "application/json";
        }

        public Service(string baseUrl,string mediaType)
        {
            this.baseUrl = baseUrl;
            this.mediaType = mediaType;
        }

        public async Task<T> GetAsync(string urlSegment)
        {
            using(var client = new HttpClient())
            {
                BuildClient(client);
                HttpResponseMessage responseMessage = await client.GetAsync(urlSegment).ConfigureAwait(false);
                if(!responseMessage.IsSuccessStatusCode)
                {
                    return default(T);
                }
                return await responseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false);
            }
        }

        private void BuildClient(HttpClient client)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));
        }

    }
}
