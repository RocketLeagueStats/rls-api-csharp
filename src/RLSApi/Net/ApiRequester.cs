using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RLSApi.Exceptions;
using RLSApi.Net.Models;

namespace RLSApi.Net
{
    /// <summary>
    ///     Used to send HTTP requests to https://api.rocketleaguestats.com.
    /// </summary>
    internal class ApiRequester : IDisposable
    {
        private readonly HttpClient _client;
        private readonly Func<HttpResponseMessage, Task> _httpExceptionHandler;

        public ApiRequester(string apiKey, Func<HttpResponseMessage, Task> exceptionHandler = null)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Constants.ApiUrl),
                DefaultRequestHeaders =
                {
                    { "Authorization", apiKey }
                }
            };

            _httpExceptionHandler = exceptionHandler ?? (async response =>
            {
                try
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        throw new RLSApiException($"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}), there was no error message available.")
                        {
                            HttpStatusCode = (int)response.StatusCode
                        };
                    }

                    var error = JsonConvert.DeserializeObject<Error>(errorMessage);

                    throw new RLSApiException($"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}), RLS: '{error.Message}'.")
                    {
                        HttpStatusCode = (int)response.StatusCode,
                        RlsError = error
                    };
                }
                catch (JsonException e)
                {
                    throw new RLSApiException($"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}), we were unable to parse the error message.", e)
                    {
                        HttpStatusCode = (int)response.StatusCode
                    };
                }
            });
        }

        public async Task<T> Get<T>(string relativeUrl)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl))
                return await SendAndReceiveAsync<T>(request);

        }



        public async Task<T> Post<T>(string relativeUrl, object data)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl))
            {
                var requestData = JsonConvert.SerializeObject(data, Formatting.None);
                request.Content = new StringContent(requestData, Encoding.UTF8, "application/json");
                return await SendAndReceiveAsync<T>(request);

            }
        }

        private async Task<T> SendAndReceiveAsync<T>(HttpRequestMessage request)
        {
            var response = await SendAsync(request);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            await _httpExceptionHandler.Invoke(response);
            return default(T);
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
