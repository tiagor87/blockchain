using Blockchain.Api.TransactionAgg.Domain.ValueObjects;
using Blockchain.Api.WalletAgg.ValueObjects;

using TheNoobs.AggregateRoot;

namespace Blockchain.Api.TransactionAgg.Domain.Entities;

public interface ITransactionView
{
    string Id { get; }
    string From { get; }
    string To { get; }
    decimal Amount { get; }
}

public class Transaction : AggregateRoot<string>
{
    private Transaction() : base(Guid.CreateVersion7().ToString())
    {
        From = null!;
        To = null!;
        Amount = null!;
    }

    protected Transaction(Address from, Address to, Money amount) : this()
    {
        From = from;
        To = to;
        Amount = amount;
    }

    public Address From { get; }
    public Address To { get; }
    public Money Amount { get; }

    public ITransactionView ToView()
    {
        return new TransactionView(this);
    }
    
    private class TransactionView(Transaction transaction) : ITransactionView
    {
        public string Id => transaction.Id;
        public string From => transaction.From.ToString();
        public string To => transaction.To.ToString();
        public decimal Amount => transaction.Amount.AsDecimal();
    }
}