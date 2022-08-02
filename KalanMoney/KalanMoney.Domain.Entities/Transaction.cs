using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(decimal amount, Description description, Category category) 
        : this(new Guid().ToString(), amount, description, category, TimeStamp.CreateNow())
    { }
    
    public Transaction(string id, decimal amount, Description description, Category category, TimeStamp timeStamp) : base(id)
    {
        Amount = amount;
        Description = description;
        Category = category;
        TimeStamp = timeStamp;
    }
    
    public decimal Amount { get; }

    public Description Description { get; }
    
    public Category Category { get; }

    public TimeStamp TimeStamp { get;  }
}