using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CosmosSharp.Models;
using CosmosSharp.Types;
using NBitcoin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CosmosSharp
{
    public class OrderedContractResolver : DefaultContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }
    
    public class Wallet
    {
        const string COSMOS_PREFIX = "cosmos";
        const string COSMOS_PATH = "m/44'/118'/0'/0/0";

        public Key PrivateKey {get;set;}
        public NBitcoin.PubKey PublicKey {get;set;}
        public string Address {get; set;}   

        public static Wallet FromMasterKey(string masterKey)
        {
            BitcoinExtKey bitcoinExtKey = new BitcoinExtKey(masterKey);
            return new Wallet(bitcoinExtKey);
        }

        public static Wallet FromMnemonic(string mnemonic)
        {
            Mnemonic mneumonic = new Mnemonic(mnemonic);
            return new Wallet(mneumonic);
        }

        Wallet(BitcoinExtKey bitcoinExtKey)
        {
            ExtKey masterKey = bitcoinExtKey.ExtKey;
            ExtKey key = masterKey.Derive(new KeyPath(COSMOS_PATH));
            PrivateKey = key.PrivateKey;
            PublicKey = PrivateKey.PubKey.Compress();
            Address = GetAddress();
        }
        
        Wallet(Mnemonic mneumonic)
        {
            byte[] seed = mneumonic.DeriveSeed("");
            ExtKey masterKey = new ExtKey(seed);

            ExtKey key = masterKey.Derive(new KeyPath(COSMOS_PATH));
            PrivateKey = key.PrivateKey;
            PublicKey = PrivateKey.PubKey.Compress();
            Address = GetAddress();
        }

        string GetAddress()
        {
            var pubBytes = PublicKey.ToBytes();
            var hash1 =  NBitcoin.Crypto.Hashes.SHA256(pubBytes);
            var hash2 = NBitcoin.Crypto.Hashes.RIPEMD160(hash1, hash1.Length);
            
            return Bech32Engine.Encode("cosmos", hash2);
        }

        byte[] toCanonicalJSONBytes(StdSignMsg signMsg){
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new OrderedContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            string msg = JsonConvert.SerializeObject(signMsg, settings);
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            return msgBytes;
        }
        byte[] toCanonicalJSONBytes<TMsg>(StdSignMsg<TMsg> signMsg){
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new OrderedContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            string msg = JsonConvert.SerializeObject(signMsg, settings);
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            return msgBytes;
        }
        byte[] sha256(byte[] msgBytes){
            var hash = SHA256.Create().ComputeHash(msgBytes);
            return hash;
        }
        string secp256k1Sign(byte[] hashed){
            NBitcoin.Crypto.ECDSASignature signature = PrivateKey.Sign(new uint256(hashed), useLowR: false);
            CosmosECDSASignature cosmosSignature = new CosmosECDSASignature(signature);
            return cosmosSignature.StringSignature;
        }

        public StdTx Sign(StdSignMsg tx)
        {
            byte[] msgBytes = toCanonicalJSONBytes(tx);
            byte[] hashed = sha256(msgBytes);
            string cosmosSignature = secp256k1Sign(hashed);

            StdTx signedTx = new StdTx()
            {
                Fee = tx.Fee,
                Memo = tx.Memo,
                Msg = tx.Msg,
                Signatures = new List<StdSignature>(){
                    new StdSignature(){
                        SignatureInfo = cosmosSignature,
                        PubKey = new Types.PubKey(){
                            Type = "tendermint/PubKeySecp256k1",
                            Value = Convert.ToBase64String(this.PublicKey.ToBytes())
                        }
                    }
                }
            };

            return signedTx;
        }

        public StdTx<TMsg> Sign<TMsg>(StdSignMsg<TMsg> tx)
        {
            byte[] msgBytes = toCanonicalJSONBytes<TMsg>(tx);
            byte[] hashed = sha256(msgBytes);
            string cosmosSignature = secp256k1Sign(hashed);

            StdTx<TMsg> signedTx = new StdTx<TMsg>()
            {
                Fee = tx.Fee,
                Memo = tx.Memo,
                Msg = tx.Msg,
                Signatures = new List<StdSignature>(){
                    new StdSignature(){
                        SignatureInfo = cosmosSignature,
                        PubKey = new Types.PubKey(){
                            Type = "tendermint/PubKeySecp256k1",
                            Value = Convert.ToBase64String(this.PublicKey.ToBytes())
                        }
                    }
                }
            };

            return signedTx;
        }

        bool verifySignature(StdTx tx, SignMeta meta, StdSignature signature) {
            var signatureBytes = Convert.FromBase64String(signature.SignatureInfo);
            if(signature.PubKey.Type == Types.PubKey.PubKeySecp256k1) 
            {
                var publicKey = Convert.FromBase64String(signature.PubKey.PubKeySecp256k1Value);

                StdSignMsg signMsg = new StdSignMsg(tx,meta){};
                var bytes = toCanonicalJSONBytes(signMsg);
                var hash = sha256(bytes);
                
                // TODO: Get signature from string
                // this.PublicKey.Verify(hash, )
                throw new NotImplementedException();
            } 

            if(signature.PubKey.Type == Types.PubKey.PubKeyMultisigThreshold) 
            {
                // TODO: Implement this
                throw new NotImplementedException();
            }

            throw new NotSupportedException();
        }

        // Verify a transaction against a signature
        public bool VerifyTx(StdTx tx,SignMeta meta)
        {
            foreach (var signature in tx.Signatures)
            {
                var isverified = verifySignature(tx, meta, signature);
                if(!isverified) return false;
            }
            return true;
        }

    }
}