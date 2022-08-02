using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

public class CategoryTest
{
    [Fact]
    public void Create_category_successfully()
    {
        // Arrange
        const string expected = "Salary";

        // Act
        var result = Category.Create(expected);

        // Assert
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("Salary its to long.", "Salary its to l")]
    [InlineData("Salaryweqeqweqweeewqeweqqe.", "Salaryweqeqweqw")]
    public void Name_category_contains_invalid_length_create_category_with_same_name_but_valid_lenght_of_15_successfully(string invalidName, string expected)
    {
        // Act
        var result = Category.Create(invalidName);

        // Assert
        Assert.Equal(expected, result.Value);
    }
}