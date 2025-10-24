using System.Security.Cryptography;
using System.Text;

using Blockchain.Api.Domain.BlockchainAgg.ValueObjects;

namespace Blockchain.Api.Domain.BlockchainAgg.Entities;

public interface IBlockView
{
    DateTimeOffset Timestamp { get; }
    string Hash { get; }
    long Nonce { get; }
    string? PreviousHash { get; }
}

public class Block
{

    private Block(Block? previous, long timestamp, Hash hash, int nonce)
    {
        Timestamp = timestamp;
        Hash = hash;
        Previous = previous;
        Nonce = nonce;
    }

    public static Block Genesis { get; } = Block.Create(null);
    
    #region Header
    public Hash Hash { get; }
    public int Nonce { get; }
    #endregion
    
    #region Body
    public long Timestamp { get; }
    public Block? Previous { get; }
    #endregion
    
    public IBlockView ToView()
    {
        return new BlockView(this);
    }
    
    public static Block Create(Block? previous)
    {
        long timestamp;
        int nonce = 0;
        bool isValid;
        Hash? hash;
        do
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            do
            {
                nonce++;
                hash = ValueObjects.Hash.FromValues(timestamp.ToString(), nonce.ToString(), previous?.Hash.Value ?? string.Empty);
                isValid = hash.IsValid();
            } while (!isValid && timestamp == DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        } while (!isValid);
        
        return new Block(previous, timestamp, hash, nonce);
    }

    private static ReadOnlySpan<byte> GetBytes(params string[] values)
    {
        return Encoding.UTF8.GetBytes(string.Join(':', values));
    }

    class BlockView(Block block) : IBlockView
    {
        public DateTimeOffset Timestamp => DateTimeOffset.FromUnixTimeSeconds(block.Timestamp);
        public string Hash => block.Hash.ToString();
        public long Nonce => block.Nonce;
        public string? PreviousHash => block.Previous?.Hash.ToString();
    }
}