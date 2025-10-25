namespace Blockchain.Api.Domain.WalletAgg.ValueObjects;

using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

public record PrivateKey(string Value)
{
    public static PrivateKey Create()
    {
        // Get secp256k1 curve parameters
        var curve = SecNamedCurves.GetByName("secp256k1");
        var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        
        // Generate a cryptographically secure random private key
        // Private key must be in range [1, n-1] where n is the order of the curve
        BigInteger privateKeyInt;
        var random = new SecureRandom();
        
        do
        {
            // Generate 32 random bytes (256 bits for secp256k1)
            var privateKeyBytes = new byte[32];
            random.NextBytes(privateKeyBytes);
            privateKeyInt = new BigInteger(1, privateKeyBytes);
        }
        while (privateKeyInt.CompareTo(BigInteger.One) < 0 || privateKeyInt.CompareTo(domainParams.N) >= 0);
        
        // Convert to hex string (pad to 64 characters for 32 bytes)
        var privateKeyHex = privateKeyInt.ToString(16).PadLeft(64, '0');
        
        return new PrivateKey(privateKeyHex);
    }
}