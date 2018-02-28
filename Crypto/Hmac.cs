using System.Security.Cryptography;

namespace Crypto
{
    public static class Hmac
    {
        private const int KeySize = 32;

        public static byte[] GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[KeySize];
                rng.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static byte[] ComputeHmacSha256(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
    }
}