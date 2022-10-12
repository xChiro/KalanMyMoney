using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
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
        accountQueriesRepository.Setup(x => x.GetAccountByOwner(It.IsAny<string>(), It.IsAny<DateRangeFilter>()))
            .Returns(default(FinancialAccount));

        var sut = new GetAccountDashboard.GetAccountDashboard(accountQueriesRepository.Object);

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

        var sut = new GetAccountDashboard.GetAccountDashboard(accountQueriesRepository.Object);
        var outPut = new AccountDashboardOutputMock();

        // Act
        sut.Execute(owner.SubId, outPut);

        // Assert
        Assert.NotNull(outPut.AccountDashboardResponse?.AccountTransactions);
        Assert.NotNull(outPut.AccountDashboardResponse.DashboardBalance);
        Assert.Empty(outPut.AccountDashboardResponse.AccountTransactions);
        Assert.Equal(0, outPut.AccountDashboardResponse.DashboardBalance.AccountBalance);
        Assert.Equal(0, outPut.AccountDashboardResponse.DashboardBalance.IncomeBalance);
        Assert.Equal(0, outPut.AccountDashboardResponse.DashboardBalance.OutcomeBalance);
    }

    [Fact]
    public void Request_dashboard_with_transactions_and_categories_balances_successfully()
    {
        // Arrange
        var categorySalary = Category.Create("Salary");
        var categoryGroceries = Category.Create("Groceries");

        const decimal amountSalary = 10;
        const decimal amountGroceries = -5;

        var transactions = new[]
        {
            new Transaction(amountSalary, Description.Create("Test"), categorySalary),
            new Transaction(amountGroceries, Description.Create("Test"), categoryGroceries)
        };

        var owner = CreateOwner();

        var accountId = Guid.NewGuid().ToString();
        var accountQueriesRepository = CreateAccountQueriesRepository(0, transactions,
            owner, accountId);

        var sut = new GetAccountDashboard.GetAccountDashboard(accountQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();

        // Act
        sut.Execute(owner.SubId, output);

        // Assert
        Assert.NotNull(output.AccountDashboardResponse?.AccountTransactions);
        Assert.Contains(transactions[0], output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(transactions[1], output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(output.AccountDashboardResponse.CategoriesBalances,
            pair => pair.Key == categorySalary.Value && pair.Value == amountSalary);
        Assert.Contains(output.AccountDashboardResponse.CategoriesBalances,
            pair => pair.Key == categoryGroceries.Value && pair.Value == amountGroceries);
    }

    [Fact]
    public void Request_dashboard_with_transactions_categories_with_monthly_balance()
    {
        // Arrange
        const decimal positiveAmount = 100.50m;
        const decimal negativeAmount = -50.50m;
        const decimal accountBalance = positiveAmount - negativeAmount;
        var category = Category.Create("Test");

        var transactions = new Transaction[]
        {
            new(positiveAmount, Description.Create("Test"), category),
            new(negativeAmount, Description.Create("test"), category),
        };

        var owner = CreateOwner();
        var accountId = Guid.NewGuid().ToString();

        var financialAccount = CreateFinancialAccount(accountId, transactions, owner, accountBalance);

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x => x.GetAccountByOwner(owner.SubId, It.IsAny<DateRangeFilter>()))
            .Returns(financialAccount);

        var sut = new GetAccountDashboard.GetAccountDashboard(accountQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();

        // Act
        sut.Execute(owner.SubId, output);

        // Assert
        Assert.NotNull(output.AccountDashboardResponse);
        Assert.NotNull(output.AccountDashboardResponse.DashboardBalance);
        Assert.NotNull(output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(transactions[0], output.AccountDashboardResponse.AccountTransactions);
        Assert.Contains(transactions[1], output.AccountDashboardResponse.AccountTransactions);
        Assert.True(output.AccountDashboardResponse.CategoriesBalances.ContainsKey(category.Value));
        Assert.Contains(transactions[1], output.AccountDashboardResponse.AccountTransactions);
        Assert.Equal(accountBalance, output.AccountDashboardResponse.DashboardBalance.AccountBalance);
        Assert.Equal(negativeAmount, output.AccountDashboardResponse.DashboardBalance.OutcomeBalance);
        Assert.Equal(positiveAmount, output.AccountDashboardResponse.DashboardBalance.IncomeBalance);
    }

    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepository(decimal balanceAmount,
        Transaction[]? transactions,
        Owner owner, string? accountId = null)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        var financialAccount = CreateFinancialAccount(accountId, transactions, owner, balanceAmount);

        accountQueriesRepository.Setup(x =>
                x.GetAccountByOwner(It.Is<string>(id => id == owner.SubId), It.IsAny<DateRangeFilter>()))
            .Returns(financialAccount);

        return accountQueriesRepository;
    }

    private static Owner CreateOwner(string name = "CreateOwner Name")
    {
        var owner = new Owner(Guid.NewGuid().ToString(), name);
        return owner;
    }

    private static FinancialAccount CreateFinancialAccount(string? accountId, Transaction[]? transactions, Owner owner,
        decimal balance)
    {
        accountId ??= Guid.NewGuid().ToString();

        var financialAccount = new FinancialAccount(accountId, AccountName.Create("Test"), owner, balance,
            TimeStamp.CreateNow(), transactions ?? Array.Empty<Transaction>());

        return financialAccount;
    }
}