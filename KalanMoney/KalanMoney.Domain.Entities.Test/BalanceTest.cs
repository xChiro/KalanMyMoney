using Xunit;

namespace KalanMoney.Domain.Entities.Properties.Test;

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
        var result = sut.SumAmount(transactionAmount);
        
        // Assert
        Assert.True(result == expected);
    }
}