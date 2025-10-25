namespace Blockchain.Api.TransactionAgg.Domain.ValueObjects;

public record Money
{
    protected Money(Currency currency, long value)
    {
        Currency = currency;
        Value = value;
    }
    
    public long Value { get; }
    public Currency Currency { get; }
    
    public decimal AsDecimal()
    {
        return 1.0m * Value / (decimal)Math.Pow(10, Currency.Exponent);
    }
    
    public static Money Create(Currency currency, long value)
    {
        return new Money(currency, value);
    }
}