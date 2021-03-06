using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.Entities;

public class FinancialCategory : Entity
{
    public FinancialCategory(AccountName name, string accountId, Owner owner)
    {
        Name = name;
        AccountId = accountId;
        Owner = owner;
        Balance = new Balance(0);
        Transactions = new TransactionCollection();
    }

    public FinancialCategory(string id, AccountName name, string accountId, Owner owner, decimal balance, IEnumerable<Transaction> transactions) : base(id)
    {
        Name = name;
        AccountId = accountId;
        Owner = owner;
        Balance = new Balance(balance);
        Transactions = new(transactions);
    }
    
    public AccountName Name { get; private set; }
    
    public string AccountId { get; init; }
    
    public Owner Owner { get; init; }
    
    public Balance Balance { get; private set; }
    
    public TransactionCollection Transactions { get; init; }

    public Balance AddTransaction(Transaction transaction)
    {
        Transactions.AddTransaction(transaction);
        Balance = Balance.SumAmount(transaction.Amount);

        return Balance;
    }
}