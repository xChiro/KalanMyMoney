using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class FinancialAccount : Entity
{
    public FinancialAccount(AccountName name, string ownerId, string ownerName)
    {
        Owner = new Owner(ownerId, ownerName);
        Name = name;
        Transactions = new TransactionCollection();
        CreationDate = DateTime.UtcNow;
        Balance = new Balance(0);
    }

    public FinancialAccount(string id, AccountName name, Owner owner, decimal balance, DateTime creationDate,
        IEnumerable<Transaction> transactions) : base(id)
    {
        Name = name; 
        Owner = owner;
        Balance = new Balance(balance);
        CreationDate = creationDate;
        Transactions = new TransactionCollection(transactions);
    }

    public AccountName Name { get; }
    
    public Owner Owner { get; }
    
    public Balance Balance { get; private set; }
    
    public DateTime CreationDate { get; }
    
    public TransactionCollection Transactions { get; }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public Balance AddIncomeTransaction(decimal amount, string description, string category)
    {
        var positiveAmount = Math.Abs(amount);
        return AddTransaction(positiveAmount, description, category);
    }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public Balance AddOutcomeTransaction(decimal amount, string description, string category)
    {
        var negativeAmount = -Math.Abs(amount);
        return AddTransaction(negativeAmount, description, category);
    }

    private Balance AddTransaction(decimal amount, string description, string category)
    {
        var transaction = Transactions.AddTransaction(amount, description, category);
        Balance = Balance.SumAmount(transaction.Amount);
        
        return Balance;
    }
}