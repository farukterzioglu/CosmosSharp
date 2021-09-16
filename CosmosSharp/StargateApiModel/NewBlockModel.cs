using Newtonsoft.Json;

namespace CosmosSharp.StargateApiModel
{
    public partial class BlockDetail
    {
        [JsonProperty("txs")]
        public Tx[] Txs { get; set; }

        [JsonProperty("tx_responses")]
        public TxResponse[] TxResponses { get; set; }
    }
}
