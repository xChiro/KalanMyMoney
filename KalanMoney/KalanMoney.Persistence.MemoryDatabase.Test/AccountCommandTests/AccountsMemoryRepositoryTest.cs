using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;
using Xunit;

namespace KalanMoney.Persistence.MemoryDatabase.Test.AccountCommandTests;

public class AccountsMemoryRepositoryTest
{
    private AddTransactionModel _addTransactionModel;

    [Fact]
    public void Open_a_new_account_successfully()
    {
        // Arrange
        var sut = new AccountsMemoryRepository();
        const string accountName = "Test";
        var financialAccount = CreateFinancialAccountEntity(accountName);

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
        var financialAccount = CreateFinancialAccountEntity("Test");
        var financialAccountModel = FinancialAccountModel.CreateFromFinancialAccount(financialAccount);
        
        var financialCategory = CreateFinancialCategory(financialAccount);
        var financialCategoryModel = FinancialCategoryModel.CreateFromFinancialCategory(financialCategory);
        
        financialAccountModel.CategoryModels.Add(financialCategory.Id, financialCategoryModel);
        
        var sut = new AccountsMemoryRepository(financialAccountModel);
        var transaction = new Transaction(0);
        var balance = new Balance(100.13m);
        _addTransactionModel =
            new AddTransactionModel(financialAccount.Id, balance, financialCategory.Id, balance);

        // Act
        sut.AddTransaction(_addTransactionModel, transaction);
    
        // Assert
        Assert.Equal(balance.Amount, sut.FinancialAccounts.First().Value.Balance);
        Assert.Equal(balance.Amount, sut.FinancialAccounts.First().Value.CategoryModels[financialCategory.Id].Balance);
        Assert.Contains(transaction, sut.FinancialAccounts.First().Value.CategoryModels[financialCategory.Id].Transactions);
        Assert.Contains(transaction, sut.FinancialAccounts.First().Value.Transactions);
    }

    private static FinancialCategory CreateFinancialCategory(FinancialAccount financialAccount)
    {
        var categoryId = Guid.NewGuid().ToString();
        var financialCategory = new FinancialCategory(categoryId, AccountName.Create("Test"), financialAccount.Id,
            financialAccount.Owner,
            new Balance(0), Array.Empty<Transaction>());
        return financialCategory;
    }

    private static FinancialAccount CreateFinancialAccountEntity(string name)
    {
        var accountName = AccountName.Create(name);
        var ownerId = Guid.NewGuid().ToString();
        const string ownerNameTest = "Owner Name Test";

        var financialAccount = new FinancialAccount(accountName, ownerId, ownerNameTest);
        return financialAccount;
    }
}