using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class TransactionCollection
{
    public TransactionCollection() : this(new List<Transaction>()) { } 

    public TransactionCollection(IEnumerable<Transaction> transactions)
    {
        _transactions = new Stack<Transaction>(transactions);
    }
    
    public Transaction[] Items => _transactions.ToArray();

    private readonly Stack<Transaction> _transactions;

    public Transaction? GetLastTransaction()
    {
        return _transactions.Count == 0 ? default : Items.First();
    }
    
    public Transaction AddTransaction(decimal amount, string description, string category)
    {
        var transaction = new Transaction(amount, Description.Create(description), Category.Create(category));
        
        _transactions.Push(transaction);
        return transaction;
    }
}