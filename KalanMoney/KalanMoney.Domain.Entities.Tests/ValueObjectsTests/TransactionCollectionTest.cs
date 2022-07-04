using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

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
        var transaction = new Transaction(105.43m);
        var sut = new TransactionCollection();

        // Act
        sut.AddTransaction(transaction);

        // Assert
        Assert.Single(sut.Items);
        Assert.True(sut.Items[0].TimeStamp.Value > 0);
    }
}