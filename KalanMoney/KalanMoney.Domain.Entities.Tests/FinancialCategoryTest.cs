using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests;

public class FinancialCategoryTest
{
    [Fact]
    public void Create_new_category_with_first_transaction_successfully()
    {
        // Arrange
        const decimal openBalance = 10;
        
        var name = AccountName.Create("Test_Salary");
        var accountId = Guid.NewGuid().ToString();
        var owner = new Owner(Guid.NewGuid().ToString(), "Name Test");

        var sut = new FinancialCategory(name, accountId, owner);
        const string transactionDescription = "Test Transaction";
        var openTransaction = new Transaction(openBalance, Description.Create(transactionDescription)); 
        // Act
        var result = sut.AddTransaction(openTransaction);

        // Assert
        Assert.True(result == new Balance(openBalance) && sut.Transactions.Items.Length > 0 && !string.IsNullOrEmpty(sut.Id));
        Assert.Equal(Description.Create(transactionDescription), sut.Transactions.Items.First().Description);
    }
    
    [Theory]
    [InlineData(10, -5, 5)]
    [InlineData(5, 5, 10)]
    [InlineData(-5, -5, -10)]
    public void Add_transaction_to_an_existing_category_successfully(decimal actualBalance, decimal transactionAmount, decimal expected)
    {
        // Arrange
        var accountName = AccountName.Create("Test Name");
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");
        const string transactionDescription = "Test Transaction";
        var openTransaction = new List<Transaction>
        {
            new (actualBalance, Description.Create(transactionDescription))
        };

        var sut = new FinancialCategory(Guid.NewGuid().ToString(), accountName, Guid.NewGuid().ToString(), owner, actualBalance, openTransaction);
        var secondTransaction = new Transaction(transactionAmount, Description.Create(transactionDescription));
        
        // Act
        var result = sut.AddTransaction(secondTransaction);

        // Assert 
        Assert.Equal(new Balance(expected), result);;
        Assert.Equal(Description.Create(transactionDescription), sut.Transactions.Items.First().Description);
    }
}