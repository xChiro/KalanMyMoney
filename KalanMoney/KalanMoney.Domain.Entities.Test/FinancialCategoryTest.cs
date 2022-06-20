using Xunit;

namespace KalanMoney.Domain.Entities.Properties.Test;

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
        var openTransaction = new Transaction(openBalance); 
        // Act
        var result = sut.AddTransaction(openTransaction);

        // Assert
        Assert.True(result == openBalance && sut.Transactions.Items.Length > 0 && !string.IsNullOrEmpty(sut.Id));
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
        var currentBalance = new Balance(actualBalance);
        var openTransaction = new List<Transaction>();
        openTransaction.Add(new Transaction(actualBalance));

        var sut = new FinancialCategory(Guid.NewGuid().ToString(), accountName, Guid.NewGuid().ToString(), owner, currentBalance, openTransaction);
        var secondTransaction = new Transaction(transactionAmount);
        
        // Act
        var result = sut.AddTransaction(secondTransaction);

        // Assert 
        Assert.Equivalent(expected, result);
    }
}