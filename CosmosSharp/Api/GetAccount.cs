using System.Collections.Generic;
using CosmosSharp.Types;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class BaseAccount
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coins")]
        public List<Coin> Coins { get; set; }

        [JsonProperty("public_key")]
        public PubKey PublicKey { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("sequence")]
        public uint Sequence { get; set; }
    }
}