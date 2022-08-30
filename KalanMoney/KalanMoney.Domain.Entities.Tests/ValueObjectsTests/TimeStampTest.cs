using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

public class TimeStampTest
{
    [Fact]
    public void Convert_time_stamp_to_date_time_successfully()
    {
        // Arrange
        const long stamp_date = 682215010000; // 08/15/1991 12:10:10 AM
        var expected = new DateTime(1991, 08, 15, 0, 10, 10);
        var sut = new TimeStamp(stamp_date);

        // Act
        var result = sut.ToDateTime();

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void Create_new_time_stamp_successfully()
    {
        // Arrange/Act
        var results = TimeStamp.CreateNow();

        // Assert
        Assert.NotNull(results);
    }
}