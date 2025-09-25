using TheNoobs.Results;

public class Block
{
    public BlockHeader Header { get; }
    public BlockBody Body { get; }

    private Block(BlockHeader header, BlockBody body)
    {
        Header = header;
        Body = body;
    }
    
    public static Result<Block> Create(BlockHeader header, BlockBody body)
    {
        return new Block(header, body);
    }
}