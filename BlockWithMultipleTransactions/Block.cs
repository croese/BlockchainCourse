using System;
using System.Collections.Generic;
using System.Text;
using BlockWithMultipleTransactions.Interfaces;
using Crypto;

namespace BlockWithMultipleTransactions
{
    internal class Block : IBlock
    {
        private MerkleTree merkleTree;

        public Block(int blockNumber)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
        }

        public List<ITransaction> Transactions { get; }
        public int BlockNumber { get; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public IBlock NextBlock { get; set; }

        public void AddTransaction(ITransaction transaction)
        {
            Transactions.Add(transaction);
        }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var blockheader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = merkleTree.RootNode + blockheader;

            return Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
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
            BlockHash = CalculateBlockHash(PreviousBlockHash);
        }

        public bool IsValidChain(string previousBlockHash, bool verbose)
        {
            var isValid = true;

            BuildMerkleTree();

            var newBlockHash = CalculateBlockHash(previousBlockHash);
            if (newBlockHash != BlockHash)
                isValid = false;
            else
                isValid |= PreviousBlockHash == previousBlockHash;

            PrintVerificationMessage(verbose, isValid);

            return NextBlock?.IsValidChain(newBlockHash, verbose) ?? isValid;
        }

        private void BuildMerkleTree()
        {
            merkleTree = new MerkleTree();

            foreach (var txn in Transactions)
                merkleTree.AppendLeaf(MerkleHash.Create(txn.CalculateTransactionHash()));

            merkleTree.BuildTree();
        }

        private void PrintVerificationMessage(bool verbose, bool isValid)
        {
            if (!verbose) return;
            if (!isValid)
                Console.WriteLine("Block Number " + BlockNumber + " : FAILED VERIFICATION");
            else
                Console.WriteLine("Block Number " + BlockNumber + " : PASS VERIFICATION");
        }
    }
}