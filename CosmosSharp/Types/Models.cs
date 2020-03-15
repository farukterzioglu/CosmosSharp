using System;
using System.Collections.Generic;
using CosmosSharp.Sdk;
using Newtonsoft.Json;

namespace CosmosSharp.Types
{
    public class MsgSend : IMsg
    {
        [JsonProperty("amount", Order=1)]
        public List<Coin> Amount { get; set; }

        [JsonProperty("from_address", Order=2)]
        public string FromAddress { get; set; }

        [JsonProperty("to_address", Order=3)]
        public string ToAddress { get; set; }

        public string Type()
        {
            return "send";
        }
    }

    public abstract class MsgBase
    {
        [JsonProperty("type", Order=1)]
        public string Type { get; set; }
    }

    public class Msg : MsgBase
    {
        [JsonProperty("value", Order=2)]
        public object Value { get; set; }
    }

    public class Msg<TMsg> : MsgBase
    {
        [JsonProperty("value", Order=2)]
        public TMsg Value { get; set; }
    }

    public class Coin
    {
        [JsonProperty("amount", Order=1)]
        public string Amount { get; set; }

        [JsonProperty("denom", Order=2)]
        public string Denom { get; set; }
    }

    public class StdFee 
    {
        public StdFee(){
            // This important!!! Should be empty list to be serialized correctly, for Cosmos standarts. 
            this.Amount = new List<Coin>();
        }
        [JsonProperty("amount", Order=1)]
        public List<Coin> Amount { get; set; }

        [JsonProperty("gas", Order=2)]
        public string Gas { get; set; }
    }

    public class PubKeyMultisigThreshold 
    {
        [JsonProperty("threshold", Order=1)]
        public ushort Threshold { get; set; }
        
        [JsonProperty("pubkeys", Order=2)]
        public List<PubKeySecp256k1> Pubkeys {get; set;}
    }

    public class PubKeySecp256k1
    {
        [JsonProperty("type", Order=1)]
        public string Type { get; set; }

        [JsonProperty("value", Order=2)]
        public string Value { get; set; }
    }

    public class PubKey
    {
        [JsonProperty("type", Order=1)]
        public string Type { get; set; }

        [JsonProperty("value", Order=2)]
        public object Value { get; set; }

        public static string PubKeySecp256k1 = "tendermint/PubKeySecp256k1";
        public static string PubKeyMultisigThreshold = "tendermint/PubKeyMultisigThreshold";

        public string PubKeySecp256k1Value { 
            get {
                if(this.Type != PubKeySecp256k1) throw new NotSupportedException();
                return (string) Value;  
            }
        }

        public PubKeyMultisigThreshold PubKeyMultisigThresholdValue { 
            get {
                if(this.Type != PubKeyMultisigThreshold) throw new NotSupportedException();
                return (PubKeyMultisigThreshold) Value;
            }
        }
    }

    public class StdSignature
    {
        [JsonProperty("pub_key", Order=1)]
        public PubKey PubKey { get; set; }

        [JsonProperty("signature", Order=2)]
        public string SignatureInfo { get; set; }
    }

    public abstract class StdTxBase
    {
        [JsonProperty("fee", Order=1)]
        public StdFee Fee { get; set; }

        private string _memo;

        [JsonProperty("memo", Order=2)]
        public string Memo { 
            get { return _memo; } 
            set{ _memo = value ?? ""; } // Empty string is critical for signing
        }

        [JsonProperty("signatures", Order=4)]
        public List<StdSignature> Signatures { get; set; }
    }

    public class StdTx : StdTxBase
    {
        [JsonProperty("msg", Order=3)]
        public List<Msg> Msg { get; set; }
    }

    public class StdTx<TMsg>: StdTxBase
    {
        [JsonProperty("msg", Order=3)]
        public List<Msg<TMsg>> Msg { get; set; }
    }

    public class StdSignDoc 
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("fee")]
        public StdFee Fee { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("msgs")]
        public List<byte[]> Msg { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }
    }
    
    public class SignMeta 
    {
        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("sequence")]
        public uint Sequence { get; set; }
    }

    public abstract class StdSignMsgBase
    {
        [JsonProperty("chain_id", Order=2)]
        public string ChainId { get; set; }

        [JsonProperty("account_number", Order=1)]
        public string AccountNumber { get; set; }

        [JsonProperty("sequence", Order=6)]
        public string Sequence { get; set; }
        [JsonProperty("fee", Order=3)]
        public StdFee Fee { get; set; }

        [JsonProperty("memo", Order=4)]
        public string Memo { get; set; }
    }

    public class StdSignMsg : StdSignMsgBase
    {
        public StdSignMsg(StdTx atomTransfer, SignMeta meta)
        {
            this.AccountNumber = meta.AccountNumber;
            this.ChainId = meta.ChainId;
            this.Sequence = meta.Sequence.ToString();

            this.Memo = atomTransfer.Memo;
            this.Fee = atomTransfer.Fee;
            this.Msg = atomTransfer.Msg;
        }

        [JsonProperty("msgs", Order=5)]
        public List<Msg> Msg { get; set; }
    }

    public class StdSignMsg<TMsg> : StdSignMsgBase
    {
        public StdSignMsg(StdTx<TMsg> atomTransfer, SignMeta meta)
        {
            this.AccountNumber = meta.AccountNumber;
            this.ChainId = meta.ChainId;
            this.Sequence = meta.Sequence.ToString();

            this.Memo = atomTransfer.Memo;
            this.Fee = atomTransfer.Fee;
            this.Msg = atomTransfer.Msg;
        }

        [JsonProperty("msgs", Order=5)]
        public List<Msg<TMsg>> Msg { get; set; }
    }
}