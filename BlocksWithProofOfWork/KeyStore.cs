using System;
using BlocksWithProofOfWork.Interfaces;
using Crypto;

namespace BlocksWithProofOfWork
{
    internal class KeyStore : IKeyStore
    {
        public KeyStore(byte[] key)
        {
            AuthenticatedHashKey = key;
            DigitalSignature = new DigitalSignature();
            DigitalSignature.AssignNewKey();
        }

        private DigitalSignature DigitalSignature { get; }

        public byte[] AuthenticatedHashKey { get; }

        public string SignBlock(string blockHash)
        {
            return Convert.ToBase64String(DigitalSignature.SignData(Convert.FromBase64String(blockHash)));
        }

        public bool VerifyBlock(string blockHash, string signature)
        {
            return DigitalSignature.VerifySignature(Convert.FromBase64String(blockHash),
                Convert.FromBase64String(signature));
        }
    }
}