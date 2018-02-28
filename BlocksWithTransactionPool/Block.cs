using System;
using System.Collections.Generic;
using System.Text;
using BlocksWithTransactionPool.Interfaces;
using BlocksWithTransactionPool.MerkleTree;
using Crypto;

namespace BlocksWithTransactionPool
{
    internal class Block : IBlock
    {
        private MerkleTree.MerkleTree merkleTree;

        public Block(int blockNumber)
        {
            BlockNumber = blockNumber;

            CreatedDate = DateTime.UtcNow;
            Transactions = new List<ITransaction>();
        }

        public Block(int blockNumber, IKeyStore keyStore) : this(blockNumber)
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
            BlockHash = CalculateBlockHash(PreviousBlockHash);

            if (KeyStore != null)
                BlockSignature = KeyStore.SignBlock(BlockHash);
        }

        public bool IsValidChain(string previousBlockHash, bool verbose)
        {
            var isValid = true;
            var validSignature = false;

            BuildMerkleTree();

            validSignature = KeyStore.VerifyBlock(BlockHash, BlockSignature);

            var newBlockHash = CalculateBlockHash(previousBlockHash);

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
    }
}