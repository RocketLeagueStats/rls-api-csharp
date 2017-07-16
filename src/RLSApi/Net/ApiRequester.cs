using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RLSApi.Net
{
    /// <summary>
    ///     Used to send HTTP requests to https://api.rocketleaguestats.com.
    /// </summary>
    internal class ApiRequester : IDisposable
    {
        private readonly HttpClient _client;

        public ApiRequester(string apiKey)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Constants.ApiUrl),
                DefaultRequestHeaders =
                {
                    { "Authorization", apiKey }
                }
            };
        }

        public async Task<T> Get<T>(string relativeUrl)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl))
            using (var response = await SendAsync(request))
            {
                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public async Task<T> Post<T>(string relativeUrl, object data)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None), Encoding.UTF8, "application/json");

                using (var response = await SendAsync(request))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
        }

        protected virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _client.SendAsync(request);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
