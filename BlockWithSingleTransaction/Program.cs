using System;

namespace BlockWithSingleTransaction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var chain = new BlockChain();
            var block1 = new Block(0, "ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss, null);
            var block2 = new Block(1, "VBG354", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss, block1);
            var block3 = new Block(2, "XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss, block2);
            var block4 = new Block(3, "CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss, block3);
            var block5 = new Block(4, "AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss, block4);
            var block6 = new Block(5, "QAX367", 6000.00m, DateTime.Now, "FJK676", 60000, ClaimType.TotalLoss, block5);
            var block7 = new Block(6, "CGO444", 7000.00m, DateTime.Now, "LKU234", 70000, ClaimType.TotalLoss, block6);
            var block8 = new Block(7, "PLO254", 8000.00m, DateTime.Now, "VBN456", 80000, ClaimType.TotalLoss, block7);

            chain.AcceptBlock(block1);
            chain.AcceptBlock(block2);
            chain.AcceptBlock(block3);
            chain.AcceptBlock(block4);
            chain.AcceptBlock(block5);
            chain.AcceptBlock(block6);
            chain.AcceptBlock(block7);
            chain.AcceptBlock(block8);

            chain.VerifyChain();

            Console.WriteLine("");
            Console.WriteLine("");

            block4.CreatedDate = new DateTime(2017, 9, 20);

            chain.VerifyChain();

            Console.WriteLine();

            Console.Read();
        }
    }
}