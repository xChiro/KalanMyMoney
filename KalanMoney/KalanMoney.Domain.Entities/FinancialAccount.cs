using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class FinancialAccount : Entity
{
    public FinancialAccount(AccountName name, string ownerId, string ownerName)
    {
        Owner = new Owner(ownerId, ownerName);
        Name = name;
        Transactions = new TransactionCollection();
        CreationDate = TimeStamp.CreateNow();
        Balance = new Balance(0);
    }

    public FinancialAccount(string id, AccountName name, Owner owner, decimal balance, TimeStamp creationDate,
        IEnumerable<Transaction> transactions) : base(id)
    {
        Name = name; 
        Owner = owner;
        Balance = new Balance(balance);
        CreationDate = creationDate;
        Transactions = new TransactionCollection(transactions);
    }

    public AccountName Name { get; private set; }
    
    public Owner Owner { get; init; }
    
    public Balance Balance { get; private set; }
    
    public TimeStamp CreationDate { get; init; }
    
    public TransactionCollection Transactions { get; private set; }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public Balance AddIncomeTransaction(decimal amount)
    {
        var positiveAmount = Math.Abs(amount);
        Balance = AddTransaction(positiveAmount);

        return Balance;
    }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public Balance AddOutcomeTransaction(decimal amount)
    {
        var negativeAmount = -Math.Abs(amount);
        Balance = AddTransaction(negativeAmount);
        
        return Balance;
    }

    private Balance AddTransaction(decimal amount)
    {
        var transaction = new Transaction(amount);
        Transactions.AddTransaction(transaction);
        
        var balance = Balance.SumAmount(transaction.Amount);

        return balance;
    }
}