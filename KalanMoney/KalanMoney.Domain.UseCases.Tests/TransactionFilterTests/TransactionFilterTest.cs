using KalanMoney.Domain.UseCases.Repositories.Models;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.TransactionFilterTests;

public class TransactionFilterTest
{
    [Fact]
    void Create_transaction_filter_for_last_moth_range_utc_time_successfully()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var lastMoth =  DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));

        // Act
        var sut = TransactionFilter.CreateMonthRangeFromUtcNow();

        // Assert
        Assert.True(sut.From == today && sut.To == lastMoth);
    }
}