using System;
using System.Text;
using BlocksWithProofOfWork.Interfaces;
using Crypto;

namespace BlocksWithProofOfWork
{
    internal class Transaction : ITransaction
    {
        public Transaction(string claimNumber, decimal settlementAmount, DateTime settlementDate,
            string carRegistration, int mileage, ClaimType claimType)
        {
            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Mileage = mileage;
            ClaimType = claimType;
        }

        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
        public ClaimType ClaimType { get; set; }

        public string CalculateTransactionHash()
        {
            var txnHash = ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
            return Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(txnHash)));
        }
    }
}