using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(string id, decimal amount, Description description, Category category, TimeStamp timeStamp) : base(id)
    {
        Amount = amount;
        Description = description;
        Category = category;
        TimeStamp = timeStamp;
    }
    
    public Transaction(decimal amount, Description description, Category category) 
        : this(Guid.NewGuid().ToString(), amount, description, category, TimeStamp.CreateNow())
    { }

    public decimal Amount { get; }

    public Description Description { get; }
    
    public Category Category { get; }

    public TimeStamp TimeStamp { get;  }
}