using System.Collections.Generic;
using CosmosSharp.Types;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public enum BroadcastMode
    {
        block, 
        sync,
        async
    }


    public class BroadcastTxRequestBase
    {
        [JsonProperty("mode")]
        public string Mode {get; set;}
    }

    public class BroadcastTxRequest: BroadcastTxRequestBase
    {
        [JsonProperty("tx")]
        public StdTx Tx {get; set;}
    }

    public class BroadcastTxRequest<TMsg>: BroadcastTxRequestBase
    {
        [JsonProperty("tx")]
        public StdTx<TMsg> Tx {get; set;}
    }    

    public class RawLog 
    {
        public string Message { get; set; }
    }

    public class BroadcastTxResponse
    {
        public string rawLog { get; set; }
        public uint height { get; set; }
        public string transactionHash { get; set; }
        public int code { get; set; }
        public uint gas_wanted { get; set; }
        public uint gas_used { get; set; }

    }
}