using System;
using System.Collections.Generic;
using CosmosSharp.Types;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class Attribute
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
    }

    public class Log
    {
        [JsonProperty("msg_index")]
        public int MsgIndex { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("log")]
        public string LogInfo { get; set; }

        [JsonProperty("events")]
        public List<Event> Events { get; set; }
    }

    public class NewTransaction
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("txhash")]
        public string TxHash { get; set; }

        [JsonProperty("raw_log")]
        public string RawLog { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("logs")]
        public List<Log> Logs { get; set; }

        [JsonProperty("gas_wanted")]
        public string GasWanted { get; set; }

        [JsonProperty("gas_used")]
        public string GasUsed { get; set; }

        [JsonProperty("tx")]
        public Tx Tx { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("events")]
        public List<Event> Events { get; set; }
    }

    public class Tx 
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public StdTx Value { get; set; }
    }

    public class LatestBlockInfo
    {
        [JsonProperty("total_count")]
        public string TotalCount { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("page_number")]
        public string PageNumber { get; set; }

        [JsonProperty("page_total")]
        public string PageTotal { get; set; }

        [JsonProperty("limit")]
        public string Limit { get; set; }

        [JsonProperty("txs")]
        public List<NewTransaction> Txs { get; set; }
    }
}