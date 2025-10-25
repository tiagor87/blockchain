namespace Blockchain.Api.Domain.WalletAgg.ValueObjects;

using System;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

public record PublicKey(string Value)
{
    public static PublicKey FromPrivateKey(PrivateKey privateKey)
    {
        var privateKeyBytes = Convert.FromHexString(privateKey.Value);
        
        // Get secp256k1 curve parameters
        var curve = SecNamedCurves.GetByName("secp256k1");
        var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        
        // Create private key parameter from bytes
        var privateKeyInt = new BigInteger(1, privateKeyBytes);
        
        // Generate public key point by multiplying generator point G by private key
        var publicKeyPoint = domainParams.G.Multiply(privateKeyInt);
        
        // Get compressed public key bytes (33 bytes: 1 byte prefix + 32 bytes X coordinate)
        var publicKeyBytes = publicKeyPoint.GetEncoded(compressed: true);
        
        // Convert to hex string
        var publicKeyHex = Convert.ToHexString(publicKeyBytes).ToLower();
        
        return new PublicKey(publicKeyHex);
    }
}