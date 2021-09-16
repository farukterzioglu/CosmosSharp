using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosSharp.StargateApiModel
{
    public partial class NewTransaction
    {
        [JsonProperty("tx")]
        public Tx Tx { get; set; }

        [JsonProperty("tx_response")]
        public TxResponse TxResponse { get; set; }
    }

    public partial class Tx
    {
        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("auth_info")]
        public AuthInfo AuthInfo { get; set; }

        [JsonProperty("signatures")]
        public string[] Signatures { get; set; }
    }

    public partial class AuthInfo
    {
        [JsonProperty("signer_infos")]
        public SignerInfo[] SignerInfos { get; set; }

        [JsonProperty("fee")]
        public Fee Fee { get; set; }
    }

    public partial class Fee
    {
        [JsonProperty("amount")]
        public AmountDetail[] Amount { get; set; }

        [JsonProperty("gas_limit")]
        public long GasLimit { get; set; }

        [JsonProperty("payer")]
        public string Payer { get; set; }

        [JsonProperty("granter")]
        public string Granter { get; set; }
    }

    public partial class SignerInfo
    {
        [JsonProperty("public_key")]
        public PublicKey PublicKey { get; set; }

        [JsonProperty("mode_info")]
        public ModeInfo ModeInfo { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }

    public partial class ModeInfo
    {
        [JsonProperty("single")]
        public Single Single { get; set; }
    }

    public partial class Single
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }
    }

    public partial class PublicKey
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("messages")]
        public object[] Messages { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("timeout_height")]
        public long TimeoutHeight { get; set; }

        [JsonProperty("extension_options")]
        public object[] ExtensionOptions { get; set; }

        [JsonProperty("non_critical_extension_options")]
        public object[] NonCriticalExtensionOptions { get; set; }
    }

    public partial class AmountDetail
    {
        [JsonProperty("denom")]
        public string Denom { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

    public partial class TxResponse
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("txhash")]
        public string Txhash { get; set; }

        [JsonProperty("codespace")]
        public string Codespace { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("raw_log")]
        public string RawLog { get; set; }

        [JsonProperty("logs")]
        public Log[] Logs { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("gas_wanted")]
        public long GasWanted { get; set; }

        [JsonProperty("gas_used")]
        public long GasUsed { get; set; }

        [JsonProperty("tx")]
        public object Tx { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public partial class Log
    {
        [JsonProperty("msg_index")]
        public long MsgIndex { get; set; }

        [JsonProperty("log")]
        public string LogLog { get; set; }

        [JsonProperty("events")]
        public Event[] Events { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public Attribute[] Attributes { get; set; }
    }

    public partial class Attribute
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}