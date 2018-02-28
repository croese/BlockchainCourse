using System;
using System.Collections.Generic;

namespace BlocksWithTransactionPool.Interfaces
{
    internal interface IBlock
    {
        List<ITransaction> Transactions { get; }
        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }
        IBlock NextBlock { get; set; }
        string BlockSignature { get; }
        IKeyStore KeyStore { get; }

        void AddTransaction(ITransaction transaction);
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock parent);
        bool IsValidChain(string previousBlockHash, bool verbose);
    }
}