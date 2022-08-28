using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

public class DescriptionTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Try_to_create_a_description_with_invalid_value(string value)
    {
        // Act/Assert
        Assert.Throws<ArgumentNullException>(() => Description.Create(value));
    }
    
    [Fact]
    public void Create_description_successfully()
    {
        // Arrange
        const string expected = "Salary from job";

        // Act
        var result = Description.Create(expected);

        // Assert
        Assert.Equal(expected, result.Value);
    }
}