using Newtonsoft.Json;

namespace RLSApi.Net.Models
{
    public class Tier
    {
        [JsonProperty("tierId", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("tierName", Required = Required.Always)]
        public string Name { get; set; }
    }
}
