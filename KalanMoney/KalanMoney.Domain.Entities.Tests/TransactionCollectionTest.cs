using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests;

public class TransactionCollectionTest
{
    [Fact]
    public void Create_a_new_empty_transaction_collection_successfully()
    {
        // Arrange/Act
        var sut = new TransactionCollection();

        // Assert
        Assert.Empty(sut.Items);
    }

    [Fact]
    public void Add_a_transaction_to_collection_successfully()
    {
        // Arrange
        const decimal amount = 105.43m;
        const string description = "Test";
        const string category = "Salary";
        
        var sut = new TransactionCollection();

        // Act
        var result = sut.AddTransaction(amount, description, category);

        // Assert
        Assert.Single(sut.Items);
        Assert.Contains(sut.Items, transaction => transaction.Id == result.Id);
        Assert.Equal(amount, result.Amount);
        Assert.Equal(Description.Create(description), result.Description);
        Assert.Equal(Category.Create(category), result.Category);
        Assert.True(result.TimeStamp.Value > 0);
    }

    [Fact]
    public void Try_to_get_the_last_transaction_from_empty_collection_return_null()
    {
        // Arrange
        var sut = new TransactionCollection();
        
        // Act
        var transaction = sut.GetLastTransaction();

        // Assert
        Assert.Null(transaction);
    }

    [Fact]
    public void Get_last_transaction_from_a_collection_with_two_transactions_successfully()
    {
        // Arrange
        var expectedTransaction = new Transaction(10, Description.Create("First Transaction"), Category.Create("Test"));
        var transactions = new List<Transaction>()
        {
            expectedTransaction,
            new (100, Description.Create("LastTransaction"), Category.Create("Test")),
        };
        
        var sut = new TransactionCollection(transactions);
        
        // Act
        var transaction = sut.GetLastTransaction();

        // Assert
        Assert.Equal(expectedTransaction, transaction);
    }
}