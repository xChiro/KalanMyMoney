using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;
using Xunit;

namespace KalanMoney.Persistence.MemoryDatabase.Tests.AccountCommandTests;

public class AccountsMemoryRepositoryTest
{
    [Fact]
    public void Open_a_new_account_successfully()
    {
        // Arrange
        var sut = new AccountsMemoryRepository();
        const string accountName = "Test";
        var financialAccount = CreateFinancialAccount(accountName, CreateOwner("Owner Name Test"));

        // Act
        sut.OpenAccount(financialAccount);

        // Assert
        Assert.Equal(1, sut.ItemsCount());
        Assert.Equal(accountName, sut.DataBase.FinancialAccounts.First().Value.AccountName);
    }

    [Fact]
    public void Add_transaction_to_an_account_successfully()
    {
        // Arrange 
        var financialAccount = CreateFinancialAccount("Test", CreateOwner("Owner Name Test"));
        var financialAccountModel = FinancialAccountModel.CreateFromFinancialAccount(financialAccount);

        var sut = new AccountsMemoryRepository(financialAccountModel);

        const string testTransaction = "Test Transaction";
        var transaction = new Transaction(0, Description.Create(testTransaction), Category.Create("Salary"));
        var balance = new Balance(100.13m);

        // Act
        sut.StoreTransaction(financialAccount.Id, balance, transaction);

        // Asser
        Assert.Equal(balance.Amount, sut.DataBase.FinancialAccounts.First().Value.Balance);
        Assert.Equal(Description.Create(testTransaction), sut.DataBase.FinancialAccounts.First().Value.Transactions.First().Description);
        Assert.Contains(transaction, sut.DataBase.FinancialAccounts.First().Value.Transactions);
    }

    [Fact]
    public void Try_to_get_an_unexciting_account_return_null()
    {
        // Arrange
        var sut = new AccountsMemoryRepository();

        // Act
        var result = sut.GetAccountWithoutTransactions(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_an_account_by_id_successfully()
    {
        // Arrange
        var owner = CreateOwner("Owner Name Test");
        var financialCategory = CreateFinancialAccount("Test", owner);

        var sut = new AccountsMemoryRepository(FinancialAccountModel.CreateFromFinancialAccount(financialCategory));

        // Act
        var result = sut.GetAccountWithoutTransactions(financialCategory.Id, owner.SubId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Try_to_get_an_unexciting_account_by_owner_id_return_null()
    {
        // Arrange
        var financialCategory = CreateFinancialAccount("Test", CreateOwner("Owner Name Test"));
        var sut = new AccountsMemoryRepository(FinancialAccountModel.CreateFromFinancialAccount(financialCategory));

        // Act 
        var result = sut.GetAccountByOwner(Guid.NewGuid().ToString(), DateRangeFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Get_an_account_by_owner_id_successfully()
    {
        // Arrange
        var owner = CreateOwner("Owner Name Test");
        var financialCategory = CreateFinancialAccount("Test", owner);
        var sut = new AccountsMemoryRepository(FinancialAccountModel.CreateFromFinancialAccount(financialCategory));

        // Act
        var result = sut.GetAccountByOwner(owner.SubId, DateRangeFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(owner.SubId, result.Owner.SubId);
    }

    [Fact]
    public void Get_an_account_by_owner_id_with_dairy_transaction_filters_successfully()
    {
        // Arrange
        const string testTransaction = "Test Transaction";
        
        var todayTransaction = new Transaction(Guid.NewGuid().ToString(), 100, Description.Create(testTransaction), 
            Category.Create("Salary"), TimeStamp.CreateNow());
        var oldTransaction =
            new Transaction(Guid.NewGuid().ToString(), -50, Description.Create(testTransaction), 
                Category.Create("Salary"), new TimeStamp(1625847972000));
        
        var transactions = new[] {todayTransaction, oldTransaction};

        var owner = CreateOwner("Owner Name Test");
        var financialCategory = CreateFinancialAccount("Test", owner, transactions);
        var sut = new AccountsMemoryRepository(FinancialAccountModel.CreateFromFinancialAccount(financialCategory));

        // Act
        var result = sut.GetAccountByOwner(owner.SubId, DateRangeFilter.CreateMonthRangeFromUtcNow());

        // Assert
        Assert.NotNull(result);
        Assert.Contains(todayTransaction, result.Transactions.Items);
        Assert.DoesNotContain(oldTransaction, result.Transactions.Items);
    }

    private static FinancialAccount CreateFinancialAccount(string name, Owner owner,
        IEnumerable<Transaction>? transactions = null)
    {
        var accountName = AccountName.Create(name);
        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), accountName, owner,
            0, TimeStamp.CreateNow(), transactions ?? Array.Empty<Transaction>());

        return financialAccount;
    }

    private static Owner CreateOwner(string ownerNameTest)
    {
        var ownerId = Guid.NewGuid().ToString();
        var owner = new Owner(ownerId, ownerNameTest);
        return owner;
    }
}