using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosSharp.StargateApiModel
{
    public class BalanceResponse
    {
        [JsonProperty("balance")]
        public BalanceResult Balance { get; set; }
    }

    public class BalanceResult
    {
        [JsonProperty("denom")]
        public string Denom { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
