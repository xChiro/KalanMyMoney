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

    public long CreationDate { get; set; }

    public List<Transaction> Transactions { get; set; }
    
    public Dictionary<string, FinancialCategoryModel> CategoryModels { get; set; }

    public FinancialAccount ToFinancialAccount()
    {
        return new FinancialAccount(Id, Domain.Entities.ValueObjects.AccountName.Create(AccountName), 
            new Owner(OwnerId, OwnerName), Balance, TimeStamp.CreateNow(), Transactions);
    }
    
    public static FinancialAccount ToFinancialAccount(FinancialAccountModel financialAccountModel, IEnumerable<Transaction> transactions)
    {
        return new FinancialAccount(financialAccountModel.Id, Domain.Entities.ValueObjects.AccountName.Create(financialAccountModel.AccountName), 
            new Owner(financialAccountModel.OwnerId, financialAccountModel.OwnerName), financialAccountModel.Balance, TimeStamp.CreateNow(), transactions);
    }

    
    public static FinancialAccountModel CreateFromFinancialAccount(FinancialAccount financialAccount)
    {
        return new FinancialAccountModel()
        {
            Id = financialAccount.Id,
            AccountName = financialAccount.Name.Value,
            OwnerId = financialAccount.Owner.ExternalUserId,
            OwnerName = financialAccount.Owner.Name,
            Balance = financialAccount.Balance.Amount,
            CreationDate = financialAccount.CreationDate.Value,
            Transactions = financialAccount.Transactions.Items.ToList(),
            CategoryModels = new Dictionary<string, FinancialCategoryModel>()
        };
    }
}