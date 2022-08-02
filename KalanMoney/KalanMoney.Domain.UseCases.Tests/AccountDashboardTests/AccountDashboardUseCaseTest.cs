using System.Diagnostics;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.AccountDashboardTests;

public class AccountDashboardUseCaseTest
{
    [Fact]
    public void Try_to_request_a_dashboard_of_unexciting_account()
    {
        // Arrange
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x => x.GetAccountByOwner(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialAccount));
        
        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() =>
            sut.Execute(Guid.NewGuid().ToString(), new AccountDashboardOutputMock()));
    }

    [Fact]
    public void Request_dashboard_with_empty_transactions_successfully()
    {
        // Arrange
        var transaction = Array.Empty<Transaction>();
        var owner = CreateOwner();
        
        var accountId = Guid.NewGuid().ToString();
        var accountQueriesRepository = CreateAccountQueriesRepository(0, transaction, owner, accountId);

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object);
        var outPut = new AccountDashboardOutputMock();

        // Act
        sut.Execute(owner.SubId, outPut);

        // Assert
        Assert.NotNull(outPut.AccountDashboardResponse.AccountTransactions);
        Assert.Empty(outPut.AccountDashboardResponse.AccountTransactions);
    }

    [Fact]
    public void Request_dashboard_with_transactions_and_categories_balances_successfully()
    {
        // Arrange
        var categorySalary = Category.Create("Salary");
        var categoryGroceries = Category.Create("Groceries");

        const decimal amountSalary = 10;
        const decimal amountGroceries = -5;
        
        var transaction = new []
        {
            new Transaction(amountSalary, Description.Create("Test"), categorySalary),
            new Transaction(amountGroceries, Description.Create("Test"), categoryGroceries)
        };
        
        var owner = CreateOwner();
        
        var accountId = Guid.NewGuid().ToString();
        var accountQueriesRepository = CreateAccountQueriesRepository(0, transaction, 
            owner, accountId);
        
        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();
        
        // Act
        sut.Execute(owner.SubId, output);

        // Assert
        Assert.NotNull(output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(transaction[0], output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(transaction[1], output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(output.AccountDashboardResponse.CategoriesBalances, pair => pair.Key == categorySalary && pair.Value == new Balance(amountSalary));
        Assert.Contains(output.AccountDashboardResponse.CategoriesBalances, pair => pair.Key == categoryGroceries && pair.Value == new Balance(amountGroceries));
    }
    
    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepository(decimal balanceAmount, Transaction[]? transactions, 
        Owner owner, string? accountId = null)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        var financialAccount = CreateFinancialAccount(accountId, transactions, owner, balanceAmount);

        accountQueriesRepository.Setup(x =>
                x.GetAccountByOwner(It.Is<string>(id => id == owner.SubId), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        
        return accountQueriesRepository;
    }

    private static Owner CreateOwner(string name = "CreateOwner Name")
    {
        var owner = new Owner(Guid.NewGuid().ToString(), name);
        return owner;
    }

    private static FinancialAccount CreateFinancialAccount(string? accountId, Transaction[]? transactions, Owner owner, decimal balance)
    {
        accountId ??= Guid.NewGuid().ToString();
        
        var financialAccount = new FinancialAccount(accountId, AccountName.Create("Test"), owner, balance,
            TimeStamp.CreateNow(), transactions ?? Array.Empty<Transaction>());
        
        return financialAccount;
    }
}