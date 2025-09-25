using System.Security.Cryptography;
using System.Text;

using TheNoobs.Results;

namespace Blockchain.ConsoleApp;

public static class StringExtensions
{
    public static Result<string> GetHash(this string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}