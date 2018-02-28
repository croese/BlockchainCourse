using System;

namespace ProofOfWorkTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pow0 = new ProofOfWork("Mary had a little lamb", 0);
            var pow1 = new ProofOfWork("Mary had a little lamb", 1);
            var pow2 = new ProofOfWork("Mary had a little lamb", 2);
            var pow3 = new ProofOfWork("Mary had a little lamb", 3);
            var pow4 = new ProofOfWork("Mary had a little lamb", 4);
            var pow5 = new ProofOfWork("Mary had a little lamb", 5);
            var pow6 = new ProofOfWork("Mary had a little lamb", 6);

            pow0.CalculateProofOfWork();
            pow1.CalculateProofOfWork();
            pow2.CalculateProofOfWork();
            pow3.CalculateProofOfWork();
            pow4.CalculateProofOfWork();
            pow5.CalculateProofOfWork();
            pow6.CalculateProofOfWork();

            Console.Read();
        }
    }
}