using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Api.Domain.BlockchainAgg;

public interface IBlockView
{
    DateTimeOffset Timestamp { get; }
    string Hash { get; }
    long Nonce { get; }
    string? PreviousHash { get; }
}

public class Block
{

    private Block(Block? previous, long index, long timestamp, string hash, long nonce)
    {
        Index = index;
        Timestamp = timestamp;
        Hash = hash;
        Previous = previous;
        Nonce = nonce;
    }

    public static Block Genesis { get; } = Block.Create(null);
    
    #region Header
    public string Hash { get; }
    public long Nonce { get; }
    #endregion
    
    #region Body
    public long Index { get; }
    public long Timestamp { get; }
    public Block? Previous { get; }
    #endregion
    
    public string CalculateHash()
    {
        return CalculateHash(Index, Timestamp, Nonce, Previous);
    }
    
    public IBlockView ToView()
    {
        return new BlockView(this);
    }
    
    public static Block Create(Block? previous)
    {
        long index = (previous?.Index ?? 0) + 1;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        long nonce = 0;
        bool isValid;
        string? hash;
        do
        {
            nonce++;
            hash = CalculateHash(index, timestamp, nonce, previous);
            isValid = hash.StartsWith("0000");
        } while (!isValid);
        
        return new Block(previous, index, timestamp, hash, nonce);
    }
    
    private static string CalculateHash(long index, long timestamp, long nonce, Block? previous)
    {
        var hashBytes = SHA256.HashData(GetBytes(index.ToString(), timestamp.ToString(), nonce.ToString(), previous?.Hash ?? string.Empty));
        return Convert.ToHexString(hashBytes);
    }

    private static ReadOnlySpan<byte> GetBytes(params string[] values)
    {
        return Encoding.UTF8.GetBytes(string.Join(':', values));
    }

    class BlockView(Block block) : IBlockView
    {
        public DateTimeOffset Timestamp => DateTimeOffset.FromUnixTimeMilliseconds(block.Timestamp);
        public string Hash => block.Hash;
        public long Nonce => block.Nonce;
        public string? PreviousHash => block.Previous?.Hash;
    }
}