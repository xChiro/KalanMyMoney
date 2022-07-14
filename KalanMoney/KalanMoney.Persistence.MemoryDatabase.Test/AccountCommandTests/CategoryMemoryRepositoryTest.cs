using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;
using Xunit;

namespace KalanMoney.Persistence.MemoryDatabase.Test.AccountCommandTests;

public class CategoryMemoryRepositoryTest
{
    [Fact]
    public void Try_to_create_category_to_an_unexciting_account_throw_exception()
    {
        // Arrange
        var categoryId = Guid.NewGuid().ToString();
        var financialCategory = CreateFinancialCategory(categoryId, new List<Transaction>());
        var sut = new CategoryMemoryRepository();

        // Act/Assert
        Assert.Throws<KeyNotFoundException>(() => sut.CreateCategory(financialCategory));
    }

    [Fact]
    public void Create_new_category_in_account_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var financialCategory = CreateFinancialCategory(accountId, new List<Transaction>());
        var financialAccount = new FinancialAccount(accountId, AccountName.Create("Test"), financialCategory.Owner, 
            0, TimeStamp.CreateNow(), new List<Transaction>());
        var financialModel = FinancialAccountModel.CreateFromFinancialAccount(financialAccount);
        
        var sut = new CategoryMemoryRepository(financialModel);
        
        // Act
        sut.CreateCategory(financialCategory);
        
        // Assert
        Assert.Contains(sut.Database.FinancialAccounts, 
            x => x.Value.CategoryModels.Any(j => j.Value.Id == financialCategory.Id));
    }

    [Fact]
    public void Try_to_get_an_unexciting_category_returns_null()
    {
        // Arrange
        var categoryId = Guid.NewGuid().ToString();
        var sut = new CategoryMemoryRepository();

        // Act
        var result = sut.GetCategoryById(categoryId, TransactionFilter.CreateMonthRangeFromUtcNow());
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_an_category_by_id_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var category = CreateFinancialCategory(accountId, new List<Transaction>());
        var ownerId = Guid.NewGuid().ToString();
        const string ownerName = "Test Name";
        
        var financialModel = CreateFinancialAccountModel(accountId, ownerId, ownerName, new[] { category });

        var sut = new CategoryMemoryRepository(financialModel);
        
        // Act
        var resultCategory = sut.GetCategoryById(category.Id, TransactionFilter.CreateMonthRangeFromUtcNow());
        
        // Assert
        Assert.NotNull(resultCategory);
        Assert.Equal(category.Id, resultCategory.Id);
        Assert.Equal(accountId, resultCategory.AccountId);
        Assert.Equal(ownerId, resultCategory.Owner.ExternalUserId);
        Assert.Equal(ownerName, resultCategory.Owner.Name);
    }

    [Fact]
    public void Get_an_category_by_id_with_transaction_filters_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var todayTransaction = new Transaction(100.00m);
        var oldTransaction = new Transaction(Guid.NewGuid().ToString(), -50.0m, new TimeStamp(1625847972000));
        var transactions = CreateTransactions(todayTransaction, oldTransaction);

        var category = CreateFinancialCategory(accountId, transactions);
        var ownerId = Guid.NewGuid().ToString();
        const string ownerName = "Test Name";
        
        var financialModel = CreateFinancialAccountModel(accountId, ownerId, ownerName, new[] { category });

        var sut = new CategoryMemoryRepository(financialModel);
        
        // Act
        var resultCategory = sut.GetCategoryById(category.Id, TransactionFilter.CreateMonthRangeFromUtcNow());
        
        // Assert
        Assert.NotNull(resultCategory);
        Assert.DoesNotContain(oldTransaction, resultCategory.Transactions.Items);
        Assert.Contains(todayTransaction, resultCategory.Transactions.Items);
    }

    [Fact]
    public void Try_to_get_a_categories_from_unexciting_account_by_account_id_return_null()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var sut = new CategoryMemoryRepository();

        // Act
        var result = sut.GetCategoriesFromAccount(accountId, TransactionFilter.CreateMonthRangeFromUtcNow());
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_two_categories_from_account_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var firstCategory = CreateFinancialCategory(accountId, new List<Transaction>());
        var secondCategory = CreateFinancialCategory(accountId, new List<Transaction>());
        var financialAccount = CreateFinancialAccountModel(accountId, Guid.NewGuid().ToString(), "OwnerName",
                new[] {firstCategory, secondCategory});
        
        var sut = new CategoryMemoryRepository(financialAccount);

        // Act
        var response = sut.GetCategoriesFromAccount(accountId, TransactionFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Contains(response, x => x.Id == firstCategory.Id);
        Assert.Contains(response, x => x.Id == secondCategory.Id);
    }

    [Fact]
    public void Get_two_categories_with_current_month_transactions_from_account_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        
        var monthTransaction = new Transaction(10);
        var oldTransaction = new Transaction( Guid.NewGuid().ToString(), 10.0m, new TimeStamp(1625847972000));
        var returnModel = new List<Transaction>
        {
            monthTransaction,
            oldTransaction
        };

        var firstCategory = CreateFinancialCategory(accountId, returnModel);
        var secondCategory = CreateFinancialCategory(accountId, new List<Transaction>());

        var financialAccountModel = CreateFinancialAccountModel(accountId, firstCategory.Owner.ExternalUserId,
            firstCategory.Owner.Name, new[] {firstCategory, secondCategory});        
        
        var sut = new CategoryMemoryRepository(financialAccountModel);
        
        // Act
        var financialCategories = sut.GetCategoriesFromAccount(accountId, TransactionFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.NotNull(financialCategories);
        Assert.Contains(financialCategories, x => x.Id == firstCategory.Id);
        Assert.Contains(financialCategories, x => x.Id == secondCategory.Id);
        Assert.DoesNotContain(oldTransaction,
            financialCategories.First(x => x.Id == firstCategory.Id).Transactions.Items);
        Assert.Contains(monthTransaction,
            financialCategories.First(x => x.Id == firstCategory.Id).Transactions.Items);
        Assert.Empty(financialCategories.First(x => x.Id == secondCategory.Id).Transactions.Items);
        
    }

    private static List<Transaction> CreateTransactions(Transaction todayTransaction, Transaction oldTransaction)
    {
        var transactions = new List<Transaction>();
        transactions.Add(todayTransaction);
        transactions.Add(oldTransaction);
        return transactions;
    }

    private static FinancialAccountModel CreateFinancialAccountModel(string accountId, string ownerId, string ownerName,
        IEnumerable<FinancialCategory> categories)
    {
        var financialModel = new FinancialAccountModel()
        {
            Id = accountId,
            AccountName = "Test",
            Balance = 0,
            CategoryModels = new Dictionary<string, FinancialCategoryModel>(),
            Transactions = new List<Transaction>(),
            CreationDate = TimeStamp.CreateNow().Value,
            OwnerId = ownerId,
            OwnerName = ownerName
        };

        foreach (var category in categories)
        { 
            financialModel.CategoryModels.Add(category.Id, FinancialCategoryModel.CreateFromFinancialCategory(category));   
        }

        return financialModel;
    }

    private static FinancialCategory CreateFinancialCategory(string accountId, List<Transaction> returnModel)
    {
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");
        var financialCategory = new FinancialCategory(AccountName.Create("Test"), accountId, owner);

        foreach (var transaction in returnModel)
        {
            financialCategory.AddTransaction(transaction);
        } 
        
        return financialCategory;
    }
} 