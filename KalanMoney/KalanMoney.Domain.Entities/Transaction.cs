using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(decimal amount, string description)
    {
        Amount = amount;
        Description = description;
        TimeStamp = TimeStamp.CreateNow();
    }
    
    public Transaction(string id, decimal amount, string description, TimeStamp timeStamp) : base(id)
    {
        Amount = amount;
        Description = description;
        TimeStamp = timeStamp;
    }
    
    public decimal Amount { get; }

    public string Description { get; }
    
    public TimeStamp TimeStamp { get;  }
}