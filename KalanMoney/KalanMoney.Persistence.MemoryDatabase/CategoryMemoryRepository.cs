using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class CategoryMemoryRepository : ICategoryCommandsRepository, ICategoryQueriesRepository
{
    public MemoryDb Database { get; }

    public CategoryMemoryRepository() : this(new MemoryDb()) { }

    public CategoryMemoryRepository(MemoryDb memoryDb)
    {
        Database = memoryDb;
    }

    public CategoryMemoryRepository(FinancialAccountModel financialAccountModel)
    {
        Database = new MemoryDb(financialAccountModel);
    }

    public void CreateCategory(FinancialCategory category)
    {
        if (!Database.FinancialAccounts.TryGetValue(category.AccountId, out var financialAccountModel))
            throw new KeyNotFoundException();

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

        if (queryResult == null) return null;
       
        var transactions = ApplyFilterTransactions(transactionFilter, queryResult.CategoryAccountModel);
        var returnModel = CloneCategoryModel(queryResult.CategoryAccountModel, transactions);

        return returnModel.ToFinancialCategory(queryResult.FinancialAccountModel);
    }

    public FinancialCategory[]? GetCategoriesFromAccount(string accountId, TransactionFilter transactionFilter)
    {
        if (!Database.FinancialAccounts.TryGetValue(accountId, out var financialAccount)) return null;
        
        var financialCategoriesModels = financialAccount.CategoryModels
            .Select(x => x.Value).ToArray();
        
        var financialCategories = new FinancialCategory[financialCategoriesModels.Length];
        
        for(var i = 0; i < financialCategoriesModels.Length; i++)
        {
            var financialCategoriesModel = financialCategoriesModels[i];
            var transactions = ApplyFilterTransactions(transactionFilter, financialCategoriesModel);
            var clonedCategoryModel = CloneCategoryModel(financialCategoriesModel, transactions);

            financialCategories[i] = clonedCategoryModel.ToFinancialCategory(financialAccount);
        }

        return financialCategories;
    }

    private static FinancialCategoryModel CloneCategoryModel(FinancialCategoryModel financialCategoriesModel,
        IEnumerable<Transaction> transactions)
    {
        var returnModel = new FinancialCategoryModel(financialCategoriesModel)
        {
            Transactions = new List<Transaction>(transactions)
        };
        return returnModel;
    }

    private static IEnumerable<Transaction> ApplyFilterTransactions(TransactionFilter transactionFilter, FinancialCategoryModel financialCategoryModel)
    {
        var transactions = financialCategoryModel.Transactions.Where(x =>
            x.TimeStamp.ToDateTime() >= transactionFilter.From.ToDateTime(TimeOnly.MinValue) &&
            x.TimeStamp.ToDateTime() <= transactionFilter.To.ToDateTime(TimeOnly.MaxValue));
        
        return transactions;
    }
}