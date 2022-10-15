using KalanMoney.Domain.UseCases.GetMonthlyTransactions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.GetMonthlyTransactions;

public class TransactionsFiltersTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Try_to_create_a_monthly_transactions_with_invalid_month_number(int invalidMonth)
    {
        // Arrange
        const int year = 2022;

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() => new TransactionsFilters(year, invalidMonth));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_with_invalid_min_year()
    {
        // Arrange
        const int month = 12;
        var invalidYear = DateTime.MinValue.Year - 1;

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() => new TransactionsFilters(invalidYear, month));
    }

    [Fact]
    public void Try_to_get_monthly_transactions_with_invalid_max_year()
    {
        // Arrange
        const int month = 12;
        var invalidYear = DateTime.MaxValue.Year + 1;

        // Act/Assert
        Assert.Throws<IndexOutOfRangeException>(() => new TransactionsFilters(invalidYear, month));
    }
}