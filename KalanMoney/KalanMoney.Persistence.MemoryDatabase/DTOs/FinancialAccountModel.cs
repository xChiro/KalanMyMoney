using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Persistence.MemoryDatabase.DTOs;

public class FinancialAccountModel
{
    public string Id { get; set; }

    public string AccountName { get; set; }

    public string OwnerId { get; set; }
    
    public string OwnerName { get; set; }
    
    public decimal Balance  { get; set; }

    public DateTime CreationDate { get; set; }

    public List<Transaction> Transactions { get; set; }
    
    public FinancialAccount ToFinancialAccount()
    {
        return new FinancialAccount(Id, Domain.Entities.ValueObjects.AccountName.Create(AccountName), 
            new Owner(OwnerId, OwnerName), Balance, DateTime.UtcNow, Transactions);
    }
    
    public static FinancialAccount ToFinancialAccount(FinancialAccountModel financialAccountModel, IEnumerable<Transaction> transactions)
    {
        return new FinancialAccount(financialAccountModel.Id, Domain.Entities.ValueObjects.AccountName.Create(financialAccountModel.AccountName), 
            new Owner(financialAccountModel.OwnerId, financialAccountModel.OwnerName), financialAccountModel.Balance, DateTime.UtcNow, transactions);
    }

    
    public static FinancialAccountModel CreateFromFinancialAccount(FinancialAccount financialAccount)
    {
        return new FinancialAccountModel()
        {
            Id = financialAccount.Id,
            AccountName = financialAccount.Name.Value,
            OwnerId = financialAccount.Owner.SubId,
            OwnerName = financialAccount.Owner.Name,
            Balance = financialAccount.Balance.Amount,
            CreationDate = financialAccount.CreationDate,
            Transactions = financialAccount.Transactions.Items.ToList(),
        };
    }
}