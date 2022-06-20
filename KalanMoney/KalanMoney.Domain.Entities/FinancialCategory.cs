using KalanMoney.Domain.Entities.Properties;

namespace KalanMoney.Domain.Entities;

public class FinancialCategory : Entity
{
    public FinancialCategory(AccountName name, string accountId, Owner owner)
    {
        Name = name;
        AccountId = accountId;
        Owner = owner;
        Balance = new Balance();
        Transactions = new TransactionCollection();
    }

    public FinancialCategory(string id, AccountName name, string accountId, Owner owner, Balance balance, IEnumerable<Transaction> transactions) : base(id)
    {
        Name = name;
        AccountId = accountId;
        Owner = owner;
        Balance = balance;
        Transactions = new(transactions);
    }
    
    public AccountName Name { get; private set; }
    
    public string AccountId { get; init; }
    
    public Owner Owner { get; init; }
    
    public Balance Balance { get; init; }
    
    public TransactionCollection Transactions { get; init; }

    public decimal AddTransaction(Transaction transaction)
    {
        Transactions.AddTransaction(transaction);
        var balance = Balance.SumAmount(transaction.Amount);

        return balance;
    }

}