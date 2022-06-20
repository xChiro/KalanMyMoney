namespace KalanMoney.Domain.Entities.Properties;

public record TransactionCollection
{
    public TransactionCollection() : this(new List<Transaction>()) { } 

    public TransactionCollection(IEnumerable<Transaction> transactions)
    {
        _transactions = new Stack<Transaction>(transactions);
    }
    
    public Transaction[] Items => _transactions.ToArray();

    private readonly Stack<Transaction> _transactions;
    
    public void AddTransaction(Transaction transaction)
    {
        _transactions.Push(transaction);
    }
}