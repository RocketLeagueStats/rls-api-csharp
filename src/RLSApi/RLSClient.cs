using System.Threading.Tasks;
using RLSApi.Net;
using RLSApi.Net.Models;

namespace RLSApi
{
    public class RLSClient
    {
        private readonly ApiRequester _api;

        public RLSClient(string apiKey, bool throttle = true)
        {
            _api = throttle ? new ApiRequester(apiKey) : new ApiRequesterThrottle(apiKey);
        }

        public async Task<Platform[]> GetPlatformsAsync()
        {
            return await _api.Get<Platform[]>("data/platforms");
        }
    }
}
