using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class CategoryMemoryRepository : ICategoryCommandsRepository, ICategoryQueriesRepository
{
    public MemoryDb Database { get; }

    public CategoryMemoryRepository()
    {
        Database = new MemoryDb();
    }

    public CategoryMemoryRepository(FinancialAccountModel financialAccountModel)
    {
        Database = new MemoryDb(financialAccountModel);
    }

    public void CreateCategory(FinancialCategory category)
    {
        if (!Database.FinancialAccounts.TryGetValue(category.AccountId, out var financialAccountModel))
            throw new IndexOutOfRangeException();

        var categoryModel = FinancialCategoryModel.CreateFromFinancialCategory(category);
        financialAccountModel.CategoryModels.Add(categoryModel.Id, categoryModel);
    }

    public FinancialCategory? GetCategoryById(string categoryId, TransactionFilter transactionFilter)
    {
        var queryResult = Database.FinancialAccounts
            .Where(pair => pair.Value.CategoryModels.ContainsKey(categoryId))
            .Select(pair => new
            {
                FinancialAccountModel = pair.Value,
                CategoryAccountModel = pair.Value.CategoryModels[categoryId]
            }).SingleOrDefault();

        if (queryResult == null) throw new IndexOutOfRangeException();
        
        return queryResult.CategoryAccountModel.ToFinancialCategory(queryResult.FinancialAccountModel);
    }

    public FinancialCategory[]? GetCategoriesOfAccount(string accountId, TransactionFilter transactionFilter)
    {
        throw new NotImplementedException();
    }
}