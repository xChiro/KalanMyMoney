using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class Transaction : Entity
{
    public Transaction(decimal amount, Description description, Category category) 
        : this(Guid.NewGuid().ToString(), amount, description, category,DateTime.UtcNow)
    { }
    
    public Transaction(string id, decimal amount, Description description, Category category, DateTime creationDate) : base(id)
    {
        Amount = amount;
        Description = description;
        Category = category;
        CreationDate = creationDate;
    }
    
    public decimal Amount { get; }

    public Description Description { get; }
    
    public Category Category { get; }

    public DateTime CreationDate { get;  }
}