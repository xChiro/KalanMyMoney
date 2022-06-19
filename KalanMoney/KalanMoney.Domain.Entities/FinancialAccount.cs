using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Properties;

namespace KalanMoney.Domain.UseCases;

public class FinancialAccount : Entity
{
    public FinancialAccount(AccountName accountName, string ownerId, string ownerName)
    {
        Owner = new Owner(ownerId, ownerName);
        AccountName = accountName;
        Transactions = new Stack<Transaction>();
        CreationDate = TimeStamp.CreateNow();
        Balance = new Balance(0);
    }
    
    public AccountName AccountName { get; init; }
    
    public Owner Owner { get; init; }
    
    public Balance Balance { get; init; }
    
    public TimeStamp CreationDate { get; init; }
    
    public Stack<Transaction> Transactions { get; private set; }

    /// <returns>
    /// Returns new account balance.
    /// </returns>
    public decimal AddIncomeTransaction(Transaction transaction)
    {
        Balance.SumAmount(transaction.Amount);
        Transactions.Push(transaction);

        return Balance.Amount;
    }
}