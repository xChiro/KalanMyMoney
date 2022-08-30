namespace KalanMoney.Persistence.CosmosDB.DTOs;

public class TransactionDto
{
    public TransactionDto(string id, decimal amount, string description, string category, long timeStamp)
    {
        Description = description;
        Category = category;
        TimeStamp = timeStamp;
        Id = id;
        Amount = amount;
    }

    public string Id { get; }

    public decimal Amount { get; }

    public string Description { get; }

    public string Category { get; }

    public long TimeStamp { get; }
}