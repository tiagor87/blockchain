using Blockchain.Api.BlockchainAgg.Domain.ValueObjects;

using TheNoobs.AggregateRoot;

namespace Blockchain.Api.BlockchainAgg.Domain.Entities;

public interface IBlockView
{
    string Timestamp { get; }
    string Hash { get; }
    string Nonce { get; }
    string? PreviousHash { get; }
}

public class Block : AggregateRoot<string>
{
    private Block() : base(Guid.CreateVersion7().ToString())
    {
        Timestamp = null!;
        Hash = null!;
        Previous = null!;
        Nonce = null!;
    }

    private Block(Block? previous, Timestamp timestamp, Hash hash, Nonce nonce)
    {
        Timestamp = timestamp;
        Hash = hash;
        Previous = previous;
        Nonce = nonce;
    }
    
    #region Header
    public Hash Hash { get; }
    public Nonce Nonce { get; }
    #endregion
    
    #region Body
    public Timestamp Timestamp { get; }
    public Block? Previous { get; }
    #endregion

    public bool IsGenesis() => Previous is null;
    
    public IBlockView ToView()
    {
        return new BlockView(this);
    }
    
    public static Block Create(Block? previous, int difficult)
    {
        Timestamp timestamp;
        bool isValid;
        Hash? hash;
        Nonce nonce = Nonce.Create();
        do
        {
            nonce = nonce.Reset();
            timestamp = ValueObjects.Timestamp.Now();
            do
            {
                nonce = nonce.Next();
                hash = ValueObjects.Hash.FromValues(
                    timestamp.Value.ToString(),
                    nonce.ToString(),
                    previous?.Hash.Value ?? string.Empty);
                isValid = hash.IsValid(difficult);
            } while (!isValid && timestamp.IsValid());
        } while (!isValid);
        
        return new Block(previous, timestamp, hash, nonce);
    }

    private class BlockView(Block block) : IBlockView
    {
        public string Timestamp => block.Timestamp.ToString();
        public string Hash => block.Hash.ToString();
        public string Nonce => block.Nonce.ToString();
        public string? PreviousHash => block.Previous?.Hash.ToString();
    }
}