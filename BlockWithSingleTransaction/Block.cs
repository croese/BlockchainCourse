using System;
using System.Text;
using BlockWithSingleTransaction.Interfaces;
using Crypto;

namespace BlockWithSingleTransaction
{
    public class Block : IBlock
    {
        public Block(int blockNumber,
            string claimNumber,
            decimal settlementAmount,
            DateTime settlementDate,
            string carRegistration,
            int mileage,
            ClaimType claimType,
            IBlock parent)
        {
            BlockNumber = blockNumber;
            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            CarRegistration = carRegistration;
            Mileage = mileage;
            ClaimType = claimType;
            CreatedDate = DateTime.UtcNow;
            SetBlockHash(parent);
        }

        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
        public ClaimType ClaimType { get; set; }

        public int BlockNumber { get; }
        public DateTime CreatedDate { get; set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; set; }
        public IBlock NextBlock { get; set; }

        public string CalculateBlockHash(string previousBlockHash)
        {
            var txnHash = ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
            var blockHeader = BlockNumber + CreatedDate.ToString() + previousBlockHash;
            var combined = txnHash + blockHeader;

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

            BlockHash = CalculateBlockHash(PreviousBlockHash);
        }

        public bool IsValidChain(string previousBlockHash, bool verbose)
        {
            var isValid = true;

            var newBlockHash = CalculateBlockHash(previousBlockHash);
            if (newBlockHash != BlockHash)
                isValid = false;
            else
                isValid |= PreviousBlockHash == previousBlockHash;

            PrintVerificationMessage(verbose, isValid);

            return NextBlock?.IsValidChain(newBlockHash, verbose) ?? isValid;
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