using RLSApi.Net;

namespace RLSApi
{
    public class RLSClient
    {
        private ApiRequester _api;

        public RLSClient(string apiKey, bool throttle = true)
        {
            _api = throttle ? new ApiRequester(apiKey) : new ApiRequesterThrottle(apiKey);
        }
    }
}
