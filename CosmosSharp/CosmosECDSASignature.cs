/// Reference: https://github.com/Nethereum/Nethereum

using System;
using NBitcoin.Crypto;

namespace CosmosSharp.Models
{
    public class CosmosECDSASignature
    {
        public CosmosECDSASignature(ECDSASignature signature)
        {
            ECDSASignature = signature;
        }

        internal ECDSASignature ECDSASignature { get; }

        public byte[] R => ECDSASignature.R.ToByteArrayUnsigned();

        public byte[] S => ECDSASignature.S.ToByteArrayUnsigned();

        public string StringSignature => Convert.ToBase64String(To64ByteArray());

        private byte[] To64ByteArray()
        {
            var rsigPad = new byte[32];
            Array.Copy(R, 0, rsigPad, rsigPad.Length - R.Length, R.Length);

            var ssigPad = new byte[32];
            Array.Copy(S, 0, ssigPad, ssigPad.Length - S.Length, S.Length);

            return ByteUtil.Merge(rsigPad, ssigPad);
        }
    }
}