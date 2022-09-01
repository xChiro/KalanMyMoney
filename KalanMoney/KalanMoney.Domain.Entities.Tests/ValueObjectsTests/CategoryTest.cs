using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

public class CategoryTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Try_to_create_a_category_with_invalid_value(string value)
    {
        // Act/Assert
        Assert.Throws<ArgumentNullException>(() => Category.Create(value));
    }
    
    [Fact]
    public void Create_category_to_lower_successfully()
    {
        // Arrange
        const string expected = "Salary";

        // Act
        var result = Category.Create(expected);

        // Assert
        Assert.Equal(expected.ToLower(), result.Value);
    }

    [Theory]
    [InlineData("Salary its to long.", "Salary its to l")]
    [InlineData("Salaryweqeqweqweeewqeweqqe.", "Salaryweqeqweqw")]
    public void Name_category_contains_invalid_length_create_category_with_same_name_but_valid_lenght_of_15_successfully(string invalidName, string expected)
    {
        // Act
        var result = Category.Create(invalidName);

        // Assert
        Assert.Equal(expected.ToLower(), result.Value);
    }

    [Theory]
    [InlineData(" Category", "Category")]
    [InlineData("Category ", "Category")]
    [InlineData(" Category ", "Category")]
    public void Apply_trim_to_categories_successfully(string value, string expected)
    {
        // Arrange/Act
        var result = Category.Create(value);

        // Assert
        Assert.Equal(Category.Create(expected), result);
    }
}