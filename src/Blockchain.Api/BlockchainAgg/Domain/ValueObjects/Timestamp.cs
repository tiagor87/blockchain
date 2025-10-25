namespace Blockchain.Api.BlockchainAgg.Domain.ValueObjects;

public record Timestamp
{
    private Timestamp(DateTimeOffset date)
    {
        Value = date.ToUnixTimeSeconds();
    }
    
    public long Value { get; }

    public bool IsValid() => Value == DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public override string ToString()
    {
        return DateTimeOffset.FromUnixTimeSeconds(Value).ToString("O");
    }

    public static Timestamp Now()
    {
        return new Timestamp(DateTimeOffset.UtcNow);
    }
}