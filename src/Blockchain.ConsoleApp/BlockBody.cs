using System.Security.Cryptography;

using TheNoobs.Results;
using TheNoobs.Results.Extensions;
using TheNoobs.Results.Types;

public class BlockBody
{
    public BlockBody()
    {
        Data = null!;
        PreviousHash = null!;
    }
    private BlockBody(
        long sequence,
        BlockData data,
        string previousHash)
    {
        this.Sequence = sequence;
        this.Timestamp = DateTime.UtcNow.Ticks;
        this.Data = data;
        this.PreviousHash = previousHash;
    }
    
    public long Sequence { get; set; }
    public long Timestamp { get; set; }
    public BlockData Data { get; set; }
    public string PreviousHash { get; set; }
    
    public static Result<BlockBody> Create(long sequence, BlockData data, string previousHash)
    {
        return new BlockBody(sequence, data, previousHash);
    }

    public Result<byte[]> Serialize()
    {
        try
        {
            var data = Hyper.HyperSerializer.Serialize(this);
            return data.ToArray();
        }
        catch (Exception e)
        {
            return new ServerErrorFail("Failed to serialize block body", exception: e);
        }
    }

    public Result<string> GetHash()
    {
        return Serialize()
            .Bind<byte[], byte[]>(x => SHA256.HashData(x.Value))
            .Bind<byte[], string>(x => Convert.ToHexString(x.Value));
    }
}