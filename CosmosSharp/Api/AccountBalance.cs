using System.Collections.Generic;
using CosmosSharp.Types;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class AccountBalance
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("result")]
        public List<Coin> Result { get; set; }
    }
}