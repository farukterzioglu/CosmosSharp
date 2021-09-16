using System;
using System.Collections.Generic;
using CosmosSharp.Sdk;
using Newtonsoft.Json;

namespace CosmosSharp.StargateApiModel.Types
{
    public class MessageSend : IMsg
    {
        [JsonProperty("amount", Order = 1)]
        public List<Coin> Amount { get; set; }

        [JsonProperty("from_address", Order = 2)]
        public string FromAddress { get; set; }

        [JsonProperty("to_address", Order = 3)]
        public string ToAddress { get; set; }

        public string Type()
        {
            return "send";
        }
    }

    public class MultipleMessageSend : IMsg
    {
        [JsonProperty("inputs", Order = 1)]
        public List<MsjData> Inputs { get; set; }

        [JsonProperty("outputs", Order = 2)]
        public List<MsjData> Outputs { get; set; }

        public string Type()
        {
            return "multisend";
        }
    }

    public abstract class MsgBase
    {
        [JsonProperty("@type", Order = 1)]
        public string Type { get; set; }
    }

    public class MsjData
    {
        [JsonProperty("address", Order = 1)]
        public string Address { get; set; }

        [JsonProperty("coins", Order = 2)]
        public List<Coin> Amounts { get; set; }
    }

    public class Coin
    {
        [JsonProperty("amount", Order = 1)]
        public string Amount { get; set; }

        [JsonProperty("denom", Order = 2)]
        public string Denom { get; set; }
    }
}