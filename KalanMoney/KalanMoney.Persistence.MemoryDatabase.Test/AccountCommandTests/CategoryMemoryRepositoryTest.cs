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
        Assert.Throws<IndexOutOfRangeException>(() => sut.CreateCategory(financialCategory));
    }

    [Fact]
    public void Create_new_category_in_account_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var financialCategory = CreateFinancialCategory(accountId, new List<Transaction>());
        var financialAccount = new FinancialAccount(accountId, AccountName.Create("Test"), financialCategory.Owner, 
            new Balance(0), TimeStamp.CreateNow(), new List<Transaction>());
        var financialModel = FinancialAccountModel.CreateFromFinancialAccount(financialAccount);
        
        var sut = new CategoryMemoryRepository(financialModel);
        
        // Act
        sut.CreateCategory(financialCategory);
        
        // Assert
        Assert.Contains(sut.Database.FinancialAccounts, 
            x => x.Value.CategoryModels.Any(j => j.Value.Id == financialCategory.Id));
    }

    [Fact]
    public void Try_to_get_a_category_from_unexciting_category_throw_exception()
    {
        // Arrange
        var categoryId = Guid.NewGuid().ToString();
        var sut = new CategoryMemoryRepository();

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() => sut.GetCategoryById(categoryId, TransactionFilter.CreateMonthRangeFromUtcNow()));
    }

    [Fact]
    public void Get_an_category_by_id_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var category = CreateFinancialCategory(accountId, new List<Transaction>());
        var ownerId = Guid.NewGuid().ToString();
        const string ownerName = "Test Name";
        
        var financialModel = CreateFinancialAccountModel(accountId, ownerId, ownerName, category);

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
        
        var financialModel = CreateFinancialAccountModel(accountId, ownerId, ownerName, category);

        var sut = new CategoryMemoryRepository(financialModel);
        
        // Act
        var resultCategory = sut.GetCategoryById(category.Id, TransactionFilter.CreateMonthRangeFromUtcNow());
        
        // Assert
        Assert.NotNull(resultCategory);
        Assert.DoesNotContain(oldTransaction, resultCategory.Transactions.Items);
        Assert.Contains(todayTransaction, resultCategory.Transactions.Items);
    }

    private static List<Transaction> CreateTransactions(Transaction todayTransaction, Transaction oldTransaction)
    {
        var transactions = new List<Transaction>();
        transactions.Add(todayTransaction);
        transactions.Add(oldTransaction);
        return transactions;
    }

    private static FinancialAccountModel CreateFinancialAccountModel(string accountId, string ownerId, string ownerName,
        FinancialCategory category)
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
        
        financialModel.CategoryModels.Add(category.Id, FinancialCategoryModel.CreateFromFinancialCategory(category));
        
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