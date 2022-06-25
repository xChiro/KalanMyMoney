using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(decimal amount)
    {
        Amount = amount;
        TimeStamp = TimeStamp.CreateNow();
    }
    
    public Transaction(string id, decimal amount, TimeStamp timeStamp) : base(id)
    {
        Amount = amount;
        TimeStamp = timeStamp;
    }
    
    public decimal Amount { get; init; }
    
    public TimeStamp TimeStamp { get; init; }
}