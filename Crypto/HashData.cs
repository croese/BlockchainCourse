using System.Security.Cryptography;

namespace Crypto
{
    public static class HashData
    {
        public static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(toBeHashed);
            }
        }
    }
}