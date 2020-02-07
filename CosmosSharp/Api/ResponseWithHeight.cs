using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class ApiResponseWithHeight<TResult>
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("result")]
        public TResult Result { get; set; }
    }
}