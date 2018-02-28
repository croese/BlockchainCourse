namespace BlocksWithTransactionPool.Interfaces
{
    internal interface IBlockChain
    {
        void AcceptBlock(IBlock block);
        void VerifyChain();
    }
}