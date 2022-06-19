using KalanMoney.Domain.UseCases;
using Xunit;

namespace KalanMoney.Domain.Entities.Properties.Test;

public class FinancialAccountTest
{
    [Fact]
    public void Create_new_account_with_first_income_transaction_successfully()
    {
        // Arrange
        const decimal incomeAmount = 10;
        const decimal expectedBalance = 10;
        
        var incomeTransaction = new Transaction(incomeAmount);

        var accountName = AccountName.Create("Test Account");
        var sut = new FinancialAccount(accountName, Guid.NewGuid().ToString(), "Test Name");
        
        // Act
        var balance = sut.AddIncomeTransaction(incomeTransaction);
        
        // Assert
        Assert.True(balance == expectedBalance);
    }
}