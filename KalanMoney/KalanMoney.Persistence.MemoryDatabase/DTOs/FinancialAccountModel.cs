using KalanMoney.Domain.Entities;

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