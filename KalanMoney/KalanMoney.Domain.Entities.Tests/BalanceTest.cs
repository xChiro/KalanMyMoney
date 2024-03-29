using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests;

public class BalanceTest
{
    [Theory]
    [InlineData(10, -5, 5)]
    [InlineData(-5, -5, -10)]
    public void Calculate_balance_of_to_transaction_successfully(decimal initAmount, decimal transactionAmount, decimal expected)
    {
        // Arrange
        var sut = new Balance(initAmount);
        
        // Act 
        var result = sut.Add(transactionAmount);
        
        // Assert
        Assert.True(result == new Balance(expected));
    }
}