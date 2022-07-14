using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests;

public class FinancialAccountTest
{
    [Fact]
    public void Add_a_transaction_to_a_new_account_successfully()
    {
        // Arrange
        const decimal incomeAmount = 10;
        const decimal expectedBalance = 10;
        
        var accountName = AccountName.Create("Test Account");
        var sut = new FinancialAccount(accountName, Guid.NewGuid().ToString(), "Test Name");
        
        // Act
        var balance = sut.AddIncomeTransaction(incomeAmount);
        
        // Assert
        Assert.True(balance == new Balance(expectedBalance) && sut.Transactions.Items.Length > 0 && !string.IsNullOrEmpty(sut.Id));
    }

    [Theory]
    [InlineData(10, 5, 15)]
    [InlineData(-5, 5, 0)]
    [InlineData(-5, -5, 0)]
    public void Add_income_transaction_to_an_existing_account_successfully(decimal actualBalance, decimal transactionAmount, decimal expected)
    {
        // Arrange
        var accountName = AccountName.Create("Test Name");
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");
        var timeStamp = TimeStamp.CreateNow();
        var transaction = new List<Transaction> { new (actualBalance) };

        var sut = new FinancialAccount(Guid.NewGuid().ToString(), accountName, owner, actualBalance, timeStamp,
            transaction);
        
        // Act
        var result = sut.AddIncomeTransaction(transactionAmount);

        // Assert 
        Assert.Equal(new Balance(expected), result);
        Assert.Equal(new Balance(expected), sut.Balance);
    }

    [Theory]
    [InlineData(10, -5, 5)]
    [InlineData(5, 5, 0)]
    [InlineData(-5, -5, -10)]
    public void Add_outcome_transaction_to_an_existing_account_successfully(decimal actualBalance, decimal transactionAmount, decimal expected)
    {
        // Arrange
        var accountName = AccountName.Create("Test Name");
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");
        var timeStamp = TimeStamp.CreateNow();
        var transaction = new List<Transaction>();
        transaction.Add(new Transaction(actualBalance));
        
        var sut = new FinancialAccount(Guid.NewGuid().ToString(), accountName, owner, actualBalance, timeStamp,
            transaction);
        
        // Act
        var result = sut.AddOutcomeTransaction(transactionAmount);

        // Assert 
        Assert.Equal(new Balance(expected), result);
        Assert.Equal(new Balance(expected), sut.Balance);
    }

    [Theory]
    [InlineData(10, 5, -5, 10)]
    [InlineData(5, 5, 0, 10)]
    [InlineData(-5, 5, -10, -10)]
    public void Add_two_transactions_income_and_outcome_to_an_account_successfully(decimal actualBalance, decimal incomeTransactionAmount,  decimal outputTransactionAmount, decimal expected)
    {
        // Arrange
        var accountName = AccountName.Create("Test Name");
        var owner = new Owner(Guid.NewGuid().ToString(), "Test Owner");
        var timeStamp = TimeStamp.CreateNow();
        var transaction = new List<Transaction> { new (actualBalance) };

        var sut = new FinancialAccount(Guid.NewGuid().ToString(), accountName, owner, actualBalance, timeStamp,
            transaction);
        
        // Act
        sut.AddIncomeTransaction(incomeTransactionAmount);
        sut.AddOutcomeTransaction(outputTransactionAmount);

        // Assert 
        Assert.Equal(new Balance(expected), sut.Balance);
    }
}