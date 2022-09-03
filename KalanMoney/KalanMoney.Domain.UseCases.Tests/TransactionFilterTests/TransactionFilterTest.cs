using KalanMoney.Domain.UseCases.Repositories.Models;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.TransactionFilterTests;

public class TransactionFilterTest
{
    [Fact]
    void Create_transaction_filter_for_last_moth_range_utc_time_successfully()
    {
        // Arrange
        var to = DateOnly.FromDateTime(DateTime.UtcNow);
        var from =  DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));

        // Act
        var sut = DateRangeFilter.CreateMonthRangeFromUtcNow();

        // Assert
        Assert.True(sut.From == from && sut.To == to);
    }
}