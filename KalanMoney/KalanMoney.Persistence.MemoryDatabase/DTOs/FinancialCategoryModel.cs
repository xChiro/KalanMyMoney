using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Persistence.MemoryDatabase.DTOs;

public class FinancialCategoryModel
{

    public FinancialCategoryModel() { }
    
    public FinancialCategoryModel(FinancialCategoryModel financialCategoryModel)
    {
        Id = financialCategoryModel.Id;
        CategoryName = financialCategoryModel.CategoryName;
        Balance = financialCategoryModel.Balance;
        Transactions = financialCategoryModel.Transactions;
    }

    public string Id { get; set; }

    public string CategoryName { get; set; }

    public decimal Balance { get; set; }

    public List<Transaction> Transactions { get; set; }

    public FinancialCategory ToFinancialCategory(FinancialAccountModel account)
    {
        var owner = new Owner(account.OwnerId, account.OwnerName);
        var accountName = AccountName.Create(CategoryName);
        var balance = new Balance(Balance);
        
        var financialCategory = new FinancialCategory(Id, accountName, account.Id, owner,
            balance, Transactions);

        return financialCategory;
    }
    
    public static FinancialCategoryModel CreateFromFinancialCategory(FinancialCategory financialCategory)
    {
        return new FinancialCategoryModel()
        {
            Id = financialCategory.Id,
            CategoryName = financialCategory.Name.Value,
            Balance = financialCategory.Balance.Amount,
            Transactions = financialCategory.Transactions.Items.ToList(),
        };
    }
}