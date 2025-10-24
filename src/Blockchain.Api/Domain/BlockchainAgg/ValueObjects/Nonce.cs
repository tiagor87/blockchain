namespace Blockchain.Api.Domain.BlockchainAgg.ValueObjects;

public record Nonce
{
    private Nonce(int value)
    {
        Value = value;
    }
    public int Value { get; }

    public Nonce Next()
    {
        return new Nonce(Value + 1);
    }
    
    public Nonce Reset()
    {
        return new Nonce(0);
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }
    
    public static Nonce Create()
    {
        return new Nonce(0);
    }
}