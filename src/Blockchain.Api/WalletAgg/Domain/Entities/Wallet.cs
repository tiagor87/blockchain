using Blockchain.Api.WalletAgg.ValueObjects;

using TheNoobs.AggregateRoot;

namespace Blockchain.Api.WalletAgg.Domain.Entities;

public class Wallet : AggregateRoot<string>
{

    private Wallet() : base(Guid.CreateVersion7().ToString("N"))
    {
        PublicKey = null!;
        Address = null!;
    }

    protected Wallet(Address address, PublicKey publicKey, PrivateKey privateKey) : this()
    {
        PrivateKey = privateKey;
        PublicKey = publicKey;
        Address = address;
    }
    
    public PrivateKey? PrivateKey { get; }
    public PublicKey PublicKey { get; }
    public Address Address { get; }
    
    public static Wallet Create(PublicKey publicKey, PrivateKey privateKey)
    {
        return new Wallet(Address.FromPublicKey(publicKey), publicKey, privateKey);
    }
}