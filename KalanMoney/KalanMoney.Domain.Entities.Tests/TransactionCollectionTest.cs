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
        
        var transaction = new Transaction(amount, Description.Create(description), Category.Create(category));
        var sut = new TransactionCollection();

        // Act
        sut.AddTransaction(transaction);

        // Assert
        Assert.Single(sut.Items);
        Assert.Equal(sut.Items.First().Amount, amount);
        Assert.Equal(Description.Create(description), sut.Items.First().Description);
        Assert.Equal(Category.Create(category), sut.Items.First().Category);
        Assert.True(sut.Items[0].TimeStamp.Value > 0);
    }
}