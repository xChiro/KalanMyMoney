using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(decimal amount, Description description)
    {
        Amount = amount;
        Description = description;
        TimeStamp = TimeStamp.CreateNow();
    }
    
    public Transaction(string id, decimal amount, Description description, TimeStamp timeStamp) : base(id)
    {
        Amount = amount;
        Description = description;
        TimeStamp = timeStamp;
    }
    
    public decimal Amount { get; }

    public Description Description { get; }
    
    public TimeStamp TimeStamp { get;  }
}