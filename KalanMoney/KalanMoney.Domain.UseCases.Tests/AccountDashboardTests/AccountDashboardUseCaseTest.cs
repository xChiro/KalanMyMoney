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
        var owner = CreateOwner();
        
        var accountId = Guid.NewGuid().ToString();
        var accountQueriesRepository = CreateAccountQueriesRepository(0, transaction, owner, accountId);
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var outPut = new AccountDashboardOutputMock();

        // Act
        sut.Execute(owner.SubId, outPut);

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

        const string description = "Test";
        var transactions = CreateTransactions(numberOfTransactions, description, transactionsAmounts);
        var owner = CreateOwner();
        var accountId = Guid.NewGuid().ToString();
        var accountQueriesRepository = CreateAccountQueriesRepository(balance, transactions, owner, accountId);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository
            .Setup(x => x.GetCategoriesFromAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialCategory[]));

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();
        
        // Act
        sut.Execute(owner.SubId, output);

        // Assert
        Assert.Equal(accountId, output.AccountDashboardResponse.AccountId);
        Assert.NotNull(output.AccountDashboardResponse.AccountTransactions);
        Assert.NotEmpty(output.AccountDashboardResponse.AccountTransactions);
        Assert.Equal(transactionsAmounts[0], output.AccountDashboardResponse.AccountTransactions[0].Amount);
        Assert.Equal(Description.Create(description), output.AccountDashboardResponse.AccountTransactions[0].Description);
        Assert.Null(output.AccountDashboardResponse.CategoryBalanceModels);
    }
    
    [Fact]
    public void Request_a_dashboard_with_one_income_transaction_and_salary_balance_category_successfully()
    {
        // Arrange
        const decimal balance = 100.10m;
        decimal[] transactionsAmounts = { balance };
        var numberOfTransactions = transactionsAmounts.Length;

        const string description = "Test";
        var transactions = CreateTransactions(numberOfTransactions, description, transactionsAmounts);
        var accountId = Guid.NewGuid().ToString();

        var owner = CreateOwner();
        var accountQueriesRepository = CreateAccountQueriesRepository(balance, transactions, 
            owner, accountId);

        var categories = CreateSingleFinancialCategories(transactionsAmounts, transactions);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository
            .Setup(x => x.GetCategoriesFromAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(categories);

        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var output = new AccountDashboardOutputMock();
        
        // Act
        sut.Execute(owner.SubId, output);

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
        Assert.Equal(Description.Create(description), output.AccountDashboardResponse.AccountTransactions[0].Description);
    }

    private static FinancialCategory[] CreateSingleFinancialCategories(IReadOnlyList<decimal> amounts,
        IEnumerable<Transaction> transactions)
    {
        var categories = new FinancialCategory[1];
        var owner = new Owner(Guid.NewGuid().ToString(), "Test CreateOwner");

        categories[0] = new FinancialCategory(Guid.NewGuid().ToString(), AccountName.Create("Test"),
            Guid.NewGuid().ToString(), owner, amounts[0], transactions);

        return categories;
    }

    private static Transaction[] CreateTransactions(int numberOfTransactions, string description, IReadOnlyList<decimal> amounts)
    {
        var transactions = new Transaction[numberOfTransactions];

        for (var i = 0; i < numberOfTransactions; i++)
        {
            transactions[i] = new Transaction(Guid.NewGuid().ToString(), amounts[i], Description.Create(description), TimeStamp.CreateNow());
        }

        return transactions;
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