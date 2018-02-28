using System;
using System.Text;
using Crypto;

namespace ProofOfWorkTest
{
    internal class ProofOfWork
    {
        private readonly int _level;
        private readonly string _message;

        public ProofOfWork(string message, int level)
        {
            _message = message;
            _level = level;
        }

        public void CalculateProofOfWork()
        {
            var targetPrefix = new string('0', _level);
            var hash = string.Empty;
            int nonce;
            var start = DateTime.UtcNow;
            for (nonce = 0; nonce < int.MaxValue; nonce++)
            {
                var toBeHashed = $"{nonce}{_message}";
                hash = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(toBeHashed)));
                if (hash.StartsWith(targetPrefix))
                    break;
            }
            var done = DateTime.UtcNow;
            var elapsed = done.Subtract(start);

            Console.WriteLine("Difficulty Level {0} - Nonce = {1} - Elapsed = {2} - {3}",
                _level,
                nonce,
                elapsed,
                hash);
        }
    }
}