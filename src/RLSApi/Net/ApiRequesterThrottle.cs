namespace RLSApi.Net
{
    /// <summary>
    ///     Used to send throttled HTTP requests to https://api.rocketleaguestats.com.
    /// </summary>
    internal class ApiRequesterThrottle : ApiRequester
    {
        public ApiRequesterThrottle(string apiKey) : base(apiKey)
        {
        }
    }
}
