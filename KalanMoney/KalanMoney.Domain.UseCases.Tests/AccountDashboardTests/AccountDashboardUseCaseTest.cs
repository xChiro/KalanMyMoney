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
        var accountQueriesRepository = CreateAccountQueriesRepository(default);
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() =>
            sut.Execute(Guid.NewGuid().ToString(), new AccountDashboardOutputMock()));
    }

    [Fact]
    public void Request_a_dashboard_to_an_existing_account_but_with_empty_transactions_successfully()
    {
        // Arrange
        var transaction = Array.Empty<Transaction>();
        var accountQueriesRepository = CreateAccountQueriesRepository(transaction);
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var outPut = new AccountDashboardOutputMock();

        // Act
        sut.Execute(Guid.NewGuid().ToString(), outPut);

        // Assert
        Assert.Empty(outPut.AccountDashboardRequest.AccountTransactions);
    }

    [Fact]
    public void Request_a_dashboard_with_one_income_transaction_but_without_balance_category_successfully()
    {
        // Arrange
        decimal[] transactionsAmounts = { 100.10m };
        var numberOfTransactions = transactionsAmounts.Length;

        var transactions = CreateTransactions(numberOfTransactions, transactionsAmounts);

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository
            .Setup(x => x.GetTransactions(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(transactions);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository
            .Setup(x => x.GetCategoriesOfAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialCategory[]));

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();
        var accountId = Guid.NewGuid().ToString();
        
        // Act
        sut.Execute(accountId, output);

        // Assert
        Assert.Equal(accountId, output.AccountDashboardRequest.AccountId);
        Assert.NotNull(output.AccountDashboardRequest.AccountTransactions);
        Assert.NotEmpty(output.AccountDashboardRequest.AccountTransactions);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardRequest.AccountTransactions[0].Amount);
        Assert.Null(output.AccountDashboardRequest.CategoryBalanceModels);
    }
    
    [Fact]
    public void Request_a_dashboard_with_one_income_transaction_and_salary_balance_category_successfully()
    {
        // Arrange
        decimal[] transactionsAmounts = { 100.10m };
        var numberOfTransactions = transactionsAmounts.Length;

        var transactions = CreateTransactions(numberOfTransactions, transactionsAmounts);

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository
            .Setup(x => x.GetTransactions(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(transactions);

        var categories = CreateFinancialCategories(transactionsAmounts, transactions);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository
            .Setup(x => x.GetCategoriesOfAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(categories);

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();
        var accountId = Guid.NewGuid().ToString();
        
        // Act
        sut.Execute(accountId, output);

        // Assert
        Assert.Equal(accountId, output.AccountDashboardRequest.AccountId);
        Assert.NotNull(output.AccountDashboardRequest.AccountTransactions);
        Assert.NotEmpty(output.AccountDashboardRequest.AccountTransactions);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardRequest.AccountTransactions[0].Amount);
        Assert.NotNull(output.AccountDashboardRequest.CategoryBalanceModels);
        Assert.NotEmpty(output.AccountDashboardRequest.CategoryBalanceModels);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardRequest.CategoryBalanceModels[0].Balance);
    }

    private static FinancialCategory[] CreateFinancialCategories(IReadOnlyList<decimal> amounts,
        IEnumerable<Transaction> transactions)
    {
        var categories = new FinancialCategory[1];
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");

        categories[0] = new FinancialCategory(Guid.NewGuid().ToString(), AccountName.Create("Test"),
            Guid.NewGuid().ToString(), owner, new Balance(amounts[0]), transactions);

        return categories;
    }

    private static Transaction[] CreateTransactions(int numberOfTransactions, IReadOnlyList<decimal> amounts)
    {
        var transactions = new Transaction[numberOfTransactions];

        for (var i = 0; i < numberOfTransactions; i++)
        {
            transactions[i] = new Transaction(Guid.NewGuid().ToString(), amounts[i], TimeStamp.CreateNow());
        }

        return transactions;
    }

    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepository(Transaction[]? transaction)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x =>
                x.GetTransactions(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(transaction);
        return accountQueriesRepository;
    }
}