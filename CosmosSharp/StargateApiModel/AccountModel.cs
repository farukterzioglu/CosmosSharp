using Newtonsoft.Json;

namespace CosmosSharp.StargateApiModel
{
    public class AccountModel
    {
        [JsonProperty("account")]
        public AccountResult Account { get; set; }
    }
    
    public class AccountResult
    {
        [JsonProperty("@type")] 
        public string Type { get; set; }

        [JsonProperty("address")] 
        public string Address { get; set; }

        [JsonProperty("pub_key")] 
        public PubKeyResult PubKey { get; set; }

        [JsonProperty("account_number")] 
        public string AccountNumber { get; set; }

        [JsonProperty("sequence")] 
        public uint Sequence { get; set; }
    }

    public class PubKeyResult
    {
        [JsonProperty("@type")] 
        public string Type { get; set; }

        [JsonProperty("key")] 
        public string Key { get; set; }
    }
}