using System;

namespace BlockWithSingleTransaction.Interfaces
{
    public interface IBlock
    {
        string ClaimNumber { get; set; }
        decimal SettlementAmount { get; set; }
        DateTime SettlementDate { get; set; }
        string CarRegistration { get; set; }
        int Mileage { get; set; }
        ClaimType ClaimType { get; set; }

        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }
        IBlock NextBlock { get; set; }

        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock parent);
        bool IsValidChain(string previousBlockHash, bool verbose);
    }
}