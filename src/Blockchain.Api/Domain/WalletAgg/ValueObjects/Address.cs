using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Api.Domain.WalletAgg.ValueObjects;

public record Address(string Value)
{
    public static Address FromPublicKey(PublicKey publicKey)
    {
        var bytes = Encoding.UTF8.GetBytes(publicKey.Value);
        var hashBytes = SHA256.HashData(bytes);
        return new Address(Convert.ToHexString(hashBytes));
    }
}