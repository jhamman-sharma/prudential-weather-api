using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace prudential_weather_api.common
{
    public interface IService<T>
    {
        Task<T> GetAsync(string urlSegment);
    }

    /// <summary>
    /// HTTP REST Service generic base class. Contains REST methods who can used to call external REST API
    /// </summary>
    public abstract class Service<T> : IService<T>
    {
        /// <summary>
        /// Service base URL
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// Return type needs to return
        /// </summary>
        private readonly string mediaType;

        /// <summary>
        /// Constructor with default return type JSON
        /// </summary>
        /// <param name="baseUrl"></param>
        public Service(string baseUrl)
        {
            this.baseUrl = baseUrl;
            mediaType = "application/json";
        }

        /// <summary>
        /// Constructor with configurable media-type
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="mediaType"></param>
        public Service(string baseUrl,string mediaType)
        {
            this.baseUrl = baseUrl;
            this.mediaType = mediaType;
        }

        /// <summary>
        /// Used to call HTTP GET method 
        /// </summary>
        /// <param name="urlSegment">Service Endpoint</param>
        /// <returns>Service response</returns>
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

        // To Do - POST, PUT, DELETE HTTP methods

        /// <summary>
        /// Build default headers for request
        /// </summary>
        private void BuildClient(HttpClient client)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));
        }

    }
}
