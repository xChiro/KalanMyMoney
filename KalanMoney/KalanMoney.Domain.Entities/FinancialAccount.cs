using KalanMoney.Domain.Entities.Properties;

namespace KalanMoney.Domain.Entities;

public class FinancialAccount : Entity
{
    public FinancialAccount(AccountName accountName, string ownerId, string ownerName)
    {
        Owner = new Owner(ownerId, ownerName);
        AccountName = accountName;
        Transactions = new TransactionCollection();
        CreationDate = TimeStamp.CreateNow();
        Balance = new Balance(0);
    }

    public FinancialAccount(string id, AccountName accountName, Owner owner, Balance balance, TimeStamp creationDate,
        IEnumerable<Transaction> transactions) : base(id)
    {
        AccountName = accountName;
        Owner = owner;
        Balance = balance;
        CreationDate = creationDate;
        Transactions = new TransactionCollection(transactions);
    }
    
    public AccountName AccountName { get; private set; }
    
    public Owner Owner { get; init; }
    
    public Balance Balance { get; init; }
    
    public TimeStamp CreationDate { get; init; }
    
    public TransactionCollection Transactions { get; private set; }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public decimal AddIncomeTransaction(decimal amount)
    {
        var positiveAmount = Math.Abs(amount);
        var balance = AddTransaction(positiveAmount);

        return balance;
    }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public decimal AddOutcomeTransaction(decimal amount)
    {
        var negativeAmount = -Math.Abs(amount);
        var balance = AddTransaction(negativeAmount);
        
        return balance;
    }

    private decimal AddTransaction(decimal amount)
    {
        var transaction = new Transaction(amount);
        Transactions.AddTransaction(transaction);
        
        var balance = Balance.SumAmount(transaction.Amount);

        return balance;
    }
}