/// Reference: https://github.com/Nethereum/Nethereum

using System;
using NBitcoin.Crypto;
using NBitcoin.Secp256k1;

namespace CosmosSharp.Models
{
    public class CosmosECDSASignature
    {
        public CosmosECDSASignature(ECDSASignature signature)
        {
            ECDSASignature = signature;
        }

        internal ECDSASignature ECDSASignature { get; }

        public string StringSignature => Convert.ToBase64String(To64ByteArray());

        private byte[] To64ByteArray()
        {
            SecpECDSASignature.TryCreateFromDer(ECDSASignature.ToDER(), out SecpECDSASignature sig);
            var (r,s) = sig;

            var R = r.ToBytes();
            var S = s.ToBytes();

            var rsigPad = new byte[32];
            Array.Copy(R, 0, rsigPad, rsigPad.Length - R.Length, R.Length);

            var ssigPad = new byte[32];
            Array.Copy(S, 0, ssigPad, ssigPad.Length - S.Length, S.Length);

            return ByteUtil.Merge(rsigPad, ssigPad);
        }
    }
}