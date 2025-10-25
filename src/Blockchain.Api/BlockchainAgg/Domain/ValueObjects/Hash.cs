using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Api.BlockchainAgg.Domain.ValueObjects;

public record Hash
{
    public Hash(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public bool IsValid(int difficult)
    {
        var padding = string.Empty.PadRight(difficult, '0');
        return Value.StartsWith(padding);
    }

    public override string ToString()
    {
        return Value[^8..];
    }

    public static Hash FromValues(params string[] values)
    {
        var body = string.Join(':', values);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var hashBytes = SHA256.HashData(bodyBytes);
        return new Hash(Convert.ToHexString(hashBytes));
    }
}