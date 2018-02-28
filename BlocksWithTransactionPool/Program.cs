using System;
using Crypto;

namespace BlocksWithTransactionPool
{
    internal class Program
    {
        private static readonly TransactionPool TxnPool = new TransactionPool();

        private static void Main(string[] args)
        {
            var txn5 = SetupTransaction();
            var keyStore = new KeyStore(Hmac.GenerateKey());

            var block1 = new Block(0, keyStore);
            var block2 = new Block(1, keyStore);
            var block3 = new Block(2, keyStore);
            var block4 = new Block(3, keyStore);

            AddTransactionsToBlocksAndCalculateHashes(block1, block2, block3, block4);

            var chain = new BlockChain();
            chain.AcceptBlock(block1);
            chain.AcceptBlock(block2);
            chain.AcceptBlock(block3);
            chain.AcceptBlock(block4);

            chain.VerifyChain();

            Console.WriteLine("");
            Console.WriteLine("");

            txn5.ClaimNumber = "weqwewe";
            chain.VerifyChain();

            Console.WriteLine();
            Console.Read();
        }

        private static void AddTransactionsToBlocksAndCalculateHashes(Block block1, Block block2, Block block3,
            Block block4)
        {
            block1.AddTransaction(TxnPool.GetTransaction());
            block1.AddTransaction(TxnPool.GetTransaction());
            block1.AddTransaction(TxnPool.GetTransaction());
            block1.AddTransaction(TxnPool.GetTransaction());

            block2.AddTransaction(TxnPool.GetTransaction());
            block2.AddTransaction(TxnPool.GetTransaction());
            block2.AddTransaction(TxnPool.GetTransaction());
            block2.AddTransaction(TxnPool.GetTransaction());

            block3.AddTransaction(TxnPool.GetTransaction());
            block3.AddTransaction(TxnPool.GetTransaction());
            block3.AddTransaction(TxnPool.GetTransaction());
            block3.AddTransaction(TxnPool.GetTransaction());

            block4.AddTransaction(TxnPool.GetTransaction());
            block4.AddTransaction(TxnPool.GetTransaction());
            block4.AddTransaction(TxnPool.GetTransaction());
            block4.AddTransaction(TxnPool.GetTransaction());

            block1.SetBlockHash(null);
            block2.SetBlockHash(block1);
            block3.SetBlockHash(block2);
            block4.SetBlockHash(block3);
        }

        private static Transaction SetupTransaction()
        {
            var txn5 = new Transaction("AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss);

            TxnPool.AddTransaction(new Transaction("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("VBG354", 2000.00m, DateTime.Now, "JKH567", 20000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(txn5);
            TxnPool.AddTransaction(new Transaction("QAX367", 6000.00m, DateTime.Now, "FJK676", 60000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("CGO444", 7000.00m, DateTime.Now, "LKU234", 70000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("PLO254", 8000.00m, DateTime.Now, "VBN456", 80000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("VBG354", 2000.00m, DateTime.Now, "JKH567", 20000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("QAX367", 6000.00m, DateTime.Now, "FJK676", 60000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("CGO444", 7000.00m, DateTime.Now, "LKU234", 70000,
                ClaimType.TotalLoss));
            TxnPool.AddTransaction(new Transaction("PLO254", 8000.00m, DateTime.Now, "VBN456", 80000,
                ClaimType.TotalLoss));

            return txn5;
        }
    }
}