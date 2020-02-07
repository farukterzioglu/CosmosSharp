using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class ApiResponse<TValue>
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public TValue Value { get; set; }
    }
}