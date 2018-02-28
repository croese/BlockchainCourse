using System.Security.Cryptography;

namespace Crypto
{
    public class DigitalSignature
    {
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }

        public byte[] SignData(byte[] hashOfDataToBeSigned)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(hashOfDataToBeSigned);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);

                var rsaFormatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
    }
}