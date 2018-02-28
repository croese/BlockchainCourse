using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BlocksWithProofOfWork.Interfaces;
using BlocksWithProofOfWork.MerkleTree;
using Crypto;

namespace BlocksWithProofOfWork
{
    internal class Block : IBlock
    {
        private MerkleTree.MerkleTree merkleTree;

        public Block(int blockNumber, int miningDifficulty)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
            Difficulty = miningDifficulty;
        }

        public Block(int blockNumber, IKeyStore keyStore, int miningDifficulty) : this(blockNumber, miningDifficulty)
        {
            KeyStore = keyStore;
        }

        public List<ITransaction> Transactions { get; }
        public int BlockNumber { get; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public IBlock NextBlock { get; set; }
        public string BlockSignature { get; private set; }
        public IKeyStore KeyStore { get; }
        public int Difficulty { get; }
        public int Nonce { get; private set; }

        public void AddTransaction(ITransaction transaction)
        {
            Transactions.Add(transaction);
        }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var blockheader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = merkleTree.RootNode + blockheader;

            string completeBlockHash;

            if (KeyStore == null)
                completeBlockHash =
                    Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
            else
                completeBlockHash =
                    Convert.ToBase64String(Hmac.ComputeHmacSha256(Encoding.UTF8.GetBytes(combined),
                        KeyStore.AuthenticatedHashKey));

            return completeBlockHash;
        }

        public void SetBlockHash(IBlock parent)
        {
            if (parent != null)
            {
                PreviousBlockHash = parent.BlockHash;
                parent.NextBlock = this;
            }
            else
            {
                // genesis block
                PreviousBlockHash = null;
            }

            BuildMerkleTree();

            //BlockHash = CalculateBlockHash(PreviousBlockHash);

            BlockHash = CalculateProofOfWork(CalculateBlockHash(PreviousBlockHash));

            if (KeyStore != null)
                BlockSignature = KeyStore.SignBlock(BlockHash);
        }

        public bool IsValidChain(string previousBlockHash, bool verbose)
        {
            var isValid = true;
            var validSignature = false;

            BuildMerkleTree();

            validSignature = KeyStore.VerifyBlock(BlockHash, BlockSignature);

            var newBlockHash = CalculateProofOfWork(CalculateBlockHash(previousBlockHash));

            validSignature = KeyStore.VerifyBlock(newBlockHash, BlockSignature);

            if (newBlockHash != BlockHash)
                isValid = false;
            else
                isValid |= PreviousBlockHash == previousBlockHash;

            PrintVerificationMessage(verbose, isValid, validSignature);

            return NextBlock?.IsValidChain(newBlockHash, verbose) ?? isValid;
        }

        private void BuildMerkleTree()
        {
            merkleTree = new MerkleTree.MerkleTree();

            foreach (var txn in Transactions)
                merkleTree.AppendLeaf(MerkleHash.Create(txn.CalculateTransactionHash()));

            merkleTree.BuildTree();
        }

        private void PrintVerificationMessage(bool verbose, bool isValid, bool validSignature)
        {
            if (!verbose) return;
            if (!isValid)
                Console.WriteLine("Block Number " + BlockNumber + " : FAILED VERIFICATION");
            else
                Console.WriteLine("Block Number " + BlockNumber + " : PASS VERIFICATION");

            if (!validSignature)
                Console.WriteLine("Block Number " + BlockNumber + " : Invalid Digital Signature");
        }

        public string CalculateProofOfWork(string blockHash)
        {
            var targetPrefix = new string('0', Difficulty);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (Nonce = 0; Nonce < int.MaxValue; Nonce++)
            {
                var hashedData =
                    Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(Nonce + blockHash)));
                if (hashedData.StartsWith(targetPrefix))
                {
                    stopWatch.Stop();
                    ValueType ts = stopWatch.Elapsed;

                    Console.WriteLine("Difficulty Level {0} - Nonce = {1} - Elapsed = {2} - {3}",
                        Difficulty,
                        Nonce,
                        ts,
                        hashedData);

                    return hashedData;
                }
            }

            throw new Exception("No nonce found!");
        }
    }
}