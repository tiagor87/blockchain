public class BlockData
{
    public BlockData() { }

    public virtual string Message { get; } = Guid.NewGuid().ToString();
}