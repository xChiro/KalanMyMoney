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
        accountQueriesRepository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialAccount));
        
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
        var accountQueriesRepository = CreateAccountQueriesRepository(0, transaction);
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var outPut = new AccountDashboardOutputMock();

        // Act
        sut.Execute(Guid.NewGuid().ToString(), outPut);

        // Assert
        Assert.NotNull(outPut.AccountDashboardResponse.AccountTransactions);
        Assert.Empty(outPut.AccountDashboardResponse.AccountTransactions);
    }

    [Fact]
    public void Request_a_dashboard_with_one_income_transaction_but_without_balance_category_successfully()
    {
        // Arrange
        const decimal balance = 100.10m;
        decimal[] transactionsAmounts = { balance };
        var numberOfTransactions = transactionsAmounts.Length;

        var transactions = CreateTransactions(numberOfTransactions, transactionsAmounts);
        var accountQueriesRepository = CreateAccountQueriesRepository(balance, transactions);

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
        Assert.Equal(accountId, output.AccountDashboardResponse.AccountId);
        Assert.NotNull(output.AccountDashboardResponse.AccountTransactions);
        Assert.NotEmpty(output.AccountDashboardResponse.AccountTransactions);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardResponse.AccountTransactions[0].Amount);
        Assert.Null(output.AccountDashboardResponse.CategoryBalanceModels);
    }
    
    [Fact]
    public void Request_a_dashboard_with_one_income_transaction_and_salary_balance_category_successfully()
    {
        // Arrange
        const decimal balance = 100.10m;
        decimal[] transactionsAmounts = { balance };
        var numberOfTransactions = transactionsAmounts.Length;

        var transactions = CreateTransactions(numberOfTransactions, transactionsAmounts);
        var accountQueriesRepository = CreateAccountQueriesRepository(balance, transactions);

        var categories = CreateSingleFinancialCategories(transactionsAmounts, transactions);

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
        Assert.Equal(accountId, output.AccountDashboardResponse.AccountId);
        Assert.NotNull(output.AccountDashboardResponse.AccountTransactions);
        Assert.NotEmpty(output.AccountDashboardResponse.AccountTransactions);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardResponse.AccountTransactions[0].Amount);
        Assert.NotNull(output.AccountDashboardResponse.CategoryBalanceModels);
        Assert.NotEmpty(output.AccountDashboardResponse.CategoryBalanceModels);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardResponse.CategoryBalanceModels[0].Balance);
        Assert.NotNull(output.AccountDashboardResponse.CategoryBalanceModels[0].Name);
        Assert.NotEmpty(output.AccountDashboardResponse.CategoryBalanceModels[0].Name);
        Assert.Equal(accountId, output.AccountDashboardResponse.AccountId);
    }

    private static FinancialCategory[] CreateSingleFinancialCategories(IReadOnlyList<decimal> amounts,
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

    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepository(decimal balanceAmount, Transaction[]? transactions)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();

        var owner = new Owner(Guid.NewGuid().ToString(), "Owner Test");
        var balance = new Balance(balanceAmount);
        
        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner, balance, 
            TimeStamp.CreateNow(), transactions ?? Array.Empty<Transaction>());

        accountQueriesRepository.Setup(x =>
                x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        
        return accountQueriesRepository;
    }
}