using System;
using System.Collections.Generic;
using BlocksWithTransactionPool.Interfaces;

namespace BlocksWithTransactionPool
{
    internal class BlockChain : IBlockChain
    {
        public BlockChain()
        {
            Blocks = new List<IBlock>();
        }

        public IBlock CurrentBlock { get; private set; }
        public IBlock HeadBlock { get; private set; }

        public List<IBlock> Blocks { get; }

        public void AcceptBlock(IBlock block)
        {
            if (HeadBlock == null) // genesis block
            {
                HeadBlock = block;
                HeadBlock.PreviousBlockHash = null;
            }

            CurrentBlock = block;
            Blocks.Add(block);
        }

        public void VerifyChain()
        {
            if (HeadBlock == null)
                throw new InvalidOperationException("Genesis block not set.");

            var isValid = HeadBlock.IsValidChain(null, true);

            Console.WriteLine(isValid ? "Blockchain integrity intact." : "Blockchain integrity NOT intact.");
        }
    }
}