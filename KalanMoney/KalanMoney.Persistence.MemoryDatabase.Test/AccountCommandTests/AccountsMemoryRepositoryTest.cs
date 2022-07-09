using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;
using Xunit;

namespace KalanMoney.Persistence.MemoryDatabase.Test.AccountCommandTests;

public class AccountsMemoryRepositoryTest
{
    [Fact]
    public void Open_a_new_account_successfully()
    {
        // Arrange
        var sut = new AccountsMemoryRepository();
        const string accountName = "Test";
        var financialAccount = CreateFinancialAccount(accountName);

        // Act
        sut.OpenAccount(financialAccount);

        // Assert
        Assert.Equal(1, sut.ItemsCount());
        Assert.Equal(accountName, sut.FinancialAccounts.First().Value.AccountName);
    }

    [Fact]
    public void Add_transaction_to_an_account_successfully()
    {
        // Arrange 
        var financialAccount = CreateFinancialAccount("Test");
        var financialAccountModel = FinancialAccountModel.CreateFromFinancialAccount(financialAccount);

        var financialCategory = CreateFinancialCategory(financialAccount);
        var financialCategoryModel = FinancialCategoryModel.CreateFromFinancialCategory(financialCategory);

        financialAccountModel.CategoryModels.Add(financialCategory.Id, financialCategoryModel);

        var sut = new AccountsMemoryRepository(financialAccountModel);

        var transaction = new Transaction(0);
        var balance = new Balance(100.13m);
        var addTransactionModel =
            new AddTransactionModel(financialAccount.Id, balance, financialCategory.Id, balance);

        // Act
        sut.AddTransaction(addTransactionModel, transaction);

        // Assert
        Assert.Equal(balance.Amount, sut.FinancialAccounts.First().Value.Balance);
        Assert.Equal(balance.Amount, sut.FinancialAccounts.First().Value.CategoryModels[financialCategory.Id].Balance);
        Assert.Contains(transaction,
            sut.FinancialAccounts.First().Value.CategoryModels[financialCategory.Id].Transactions);
        Assert.Contains(transaction, sut.FinancialAccounts.First().Value.Transactions);
    }

    [Fact]
    public void Try_to_get_an_unexciting_account_returns_null()
    {
        // Arrange
        var sut = new AccountsMemoryRepository();

        // Act
        var result = sut.GetAccount(Guid.NewGuid().ToString(), TransactionFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_an_account_with_to_days_transactions_successfully()
    {
        // Arrange
        var todayTransaction = new Transaction(Guid.NewGuid().ToString(), 100, TimeStamp.CreateNow());
        var oldTransaction = new Transaction(Guid.NewGuid().ToString(), -50, new TimeStamp(1625847972000));
        var transactions = new[] { todayTransaction, oldTransaction };
        
        var financialCategory = CreateFinancialAccount("Test", transactions);

        var sut = new AccountsMemoryRepository(FinancialAccountModel.CreateFromFinancialAccount(financialCategory));

        // Act
        var result = sut.GetAccount(financialCategory.Id, TransactionFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.NotNull(result);
        Assert.Contains(todayTransaction, result.Transactions.Items);
        Assert.DoesNotContain(oldTransaction, result.Transactions.Items);
    }

    private static FinancialCategory CreateFinancialCategory(FinancialAccount financialAccount)
    {
        var categoryId = Guid.NewGuid().ToString();
        var financialCategory = new FinancialCategory(categoryId, AccountName.Create("Test"), financialAccount.Id,
            financialAccount.Owner,
            new Balance(0), Array.Empty<Transaction>());
        return financialCategory;
    }

    private static FinancialAccount CreateFinancialAccount(string name, IEnumerable<Transaction>? transactions = null)
    {
        var accountName = AccountName.Create(name);
        var ownerId = Guid.NewGuid().ToString();
        var owner = new Owner(ownerId, "Owner Name Test");

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), accountName, owner,
            new Balance(0), TimeStamp.CreateNow(), transactions ?? Array.Empty<Transaction>());

        return financialAccount;
    }
}