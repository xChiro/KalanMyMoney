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
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId, It.IsAny<TransactionFilter>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, invalidMonth, year),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_with_invalid_min_year()
    {
        // Arrange
        const int month = 12;
        var invalidYear = DateTime.MinValue.Year - 1;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId, It.IsAny<TransactionFilter>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, month, invalidYear),
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
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId, It.IsAny<TransactionFilter>()));

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, month, invalidYear),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_from_invalid_account()
    {
        // Arrange
        const int month = 12;
        const int invalidYear = 2022;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId, It.IsAny<TransactionFilter>()))
            .Throws(new KeyNotFoundException());

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<KeyNotFoundException>(() =>
            sut.Execute(new GetMonthlyTransactionsRequest(accountId, month, invalidYear),
                new GetMonthlyTransactionsOutputMock()));
    }

    [Fact]
    public void Get_monthly_transactions_successfully()
    {
        // Arrange
        var month = DateTime.Now.Month;
        var invalidYear =  DateTime.Now.Year;
        var accountId = Guid.NewGuid().ToString();

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        var expectedTransaction = new Transaction(100, Description.Create("Test"), Category.Create("Salary"));
        
        accountQueriesRepository.Setup(repository => repository.GetMonthlyTransactions(accountId, 
                It.IsAny<TransactionFilter>()))
            .Returns(new Transaction[]
            {
                expectedTransaction
            });

        var sut = new GetMonthlyTransactionsUseCase(accountQueriesRepository.Object);
        var output = new GetMonthlyTransactionsOutputMock();

        // Act
        sut.Execute(new GetMonthlyTransactionsRequest(accountId, month, invalidYear), output);

        // Assert
        Assert.Contains(expectedTransaction, output.Transactions);
    }
}