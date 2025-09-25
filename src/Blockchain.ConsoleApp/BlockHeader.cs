using System.Security.Cryptography;

using TheNoobs.Results;

public class BlockHeader
{
    public long Nonce { get; }
    public string Hash { get; }
    
    private BlockHeader(
        long nonce,
        string hash)
    {
        this.Nonce = nonce;
        this.Hash = hash;
    }

    public static Result<BlockHeader> Create(long nonce, BlockBody body)
    {
        var data = Hyper.HyperSerializer.Serialize(body);
        var hash = SHA256.HashData(data);
        var hexHash = Convert.ToHexString(hash);
        return new BlockHeader(nonce, hexHash);
    }
}