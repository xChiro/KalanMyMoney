using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.GetMonthlyTransactions;

public class GetMonthlyTransactionsTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Try_to_get_monthly_transactions_with_invalid_month_number(int invalidMonth)
    {
        // Arrange
        const int year = 2022;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository =>
            repository.GetMonthlyTransactions(accountId, It.IsAny<string>(), It.IsAny<GetTransactionsFilters>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var transactionsFilters = new TransactionsFilters(year, invalidMonth);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, Guid.NewGuid().ToString(),
                transactionsFilters), new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_with_invalid_min_year()
    {
        // Arrange
        const int month = 12;
        var invalidYear = DateTime.MinValue.Year - 1;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository =>
            repository.GetMonthlyTransactions(accountId, It.IsAny<string>(), It.IsAny<GetTransactionsFilters>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var transactionsFilters = new TransactionsFilters(invalidYear, month);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, Guid.NewGuid().ToString(), transactionsFilters),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_with_invalid_max_year()
    {
        // Arrange
        const int month = 12;
        var invalidYear = DateTime.MaxValue.Year + 1;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository =>
            repository.GetMonthlyTransactions(accountId, It.IsAny<string>(), It.IsAny<GetTransactionsFilters>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var transactionsFilters = new TransactionsFilters(invalidYear, month);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, Guid.NewGuid().ToString(), transactionsFilters),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_from_invalid_account()
    {
        // Arrange
        const int month = 12;
        const int year = 2022;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository =>
                repository.GetMonthlyTransactions(accountId, It.IsAny<string>(), It.IsAny<GetTransactionsFilters>()))
            .Throws(new KeyNotFoundException());

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var transactionsFilters = new TransactionsFilters(year, month);

        // Act/Assert
        Assert.Throws<KeyNotFoundException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, Guid.NewGuid().ToString(), transactionsFilters),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Get_monthly_transactions_successfully()
    {
        // Arrange
        var month = DateTime.Now.Month;
        var invalidYear = DateTime.Now.Year;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        var expectedTransaction = new Transaction(100, Description.Create("Test"), Category.Create("Salary"));

        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId,
                It.IsAny<string>(), It.IsAny<GetTransactionsFilters>()))
            .Returns(new[]
            {
                expectedTransaction
            });
        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var output = new GetMonthlyTransactionsOutputMock();
        var transactionsFilters = new TransactionsFilters(invalidYear, month);

        // Act
        sut.Execute(new GetMonthlyTransactionsRequest(accountId, Guid.NewGuid().ToString(),
            transactionsFilters), output);

        // Assert
        Assert.Contains(expectedTransaction, output.Transactions);
    }

    [Fact]
    public void Get_monthly_transactions_filter_by_category_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var ownerId = Guid.NewGuid().ToString();
        var category = Category.Create("Category");

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId,
                ownerId, It.IsAny<GetTransactionsFilters>()))
            .Returns(() =>
            {
                return new[]
                {
                    new Transaction(100, Description.Create("Text"), category)
                };
            });

        var transactionsFilters = new TransactionsFilters(DateTime.Today.Year, DateTime.Today.Month, category.Value);

        var request = new GetMonthlyTransactionsRequest(accountId, ownerId, transactionsFilters);
        var output = new GetMonthlyTransactionsOutputMock();
        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);

        // Act
        sut.Execute(request, output);

        // Assert
        Assert.True(output.Transactions.All(transaction => transaction.Category == category));
    }
}