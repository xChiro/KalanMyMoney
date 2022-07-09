using KalanMoney.Domain.Entities;

namespace KalanMoney.Persistence.MemoryDatabase.DTOs;

public class FinancialCategoryModel
{
    public string Id { get; set; }

    public string CategoryName { get; set; }
    
    public decimal Balance { get; set; }
    
    public List<Transaction> Transactions { get; set; }

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