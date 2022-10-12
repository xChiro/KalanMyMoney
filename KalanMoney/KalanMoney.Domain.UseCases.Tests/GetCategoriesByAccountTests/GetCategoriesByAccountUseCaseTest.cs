using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.GetCategoriesByAccount;
using KalanMoney.Domain.UseCases.Repositories;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.GetCategoriesByAccount;

public class GetCategoriesByAccountTest
{
    [Fact]
    public void Try_to_get_categories_from_unexciting_account()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountQueriesRepository>();
        accountRepositoryMock.Setup(x => x.GetCategoriesByAccount(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(() => new AccountNotFoundException());

        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();

        // Act / Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute("0", "0", output));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Try_to_get_categories_with_invalid_AccountId(string accountId)
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountQueriesRepository>();
        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();

        // Act / Assert
        Assert.Throws<ArgumentException>(() => sut.Execute(accountId, "owner id", output));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Try_to_get_categories_with_invalid_ownerId(string ownerId)
    {
        // Arrange
        const string accountId = "account_id";
        var accountRepositoryMock = new Mock<IAccountQueriesRepository>();
        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();

        // Act / Assert
        Assert.Throws<ArgumentException>(() => sut.Execute(accountId, ownerId, output));
    }


    [Fact]
    public void Get_categories_from_account_without_transactions_successfully()
    {
        // Arrange
        var account = new FinancialAccount(AccountName.Create("Test"), "ownerId", "ownerName");
        var accountRepositoryMock = GenerateAccountRepositoryMock(account);

        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();

        // Act
        sut.Execute(account.Id, account.Owner.SubId, output);

        // Assert
        Assert.Empty(output.Categories);
    }

    [Fact]
    public void Get_categories_from_account_with_two_transactions_with_same_category()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var category = Category.Create("Category");

        var transactions = new Transaction[]
        {
            new(5, Description.Create("Test"), category),
            new(5, Description.Create("Test"), category)
        };
        
        var account = GenerateFinancialAccount(accountId, transactions);

        var accountRepositoryMock = GenerateAccountRepositoryMock(account);
        
        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();
        
        // Act
        sut.Execute(accountId, account.Owner.SubId, output);

        // Assert
        Assert.Single(output.Categories);
    }

    [Fact]
    public void Get_categories_from_account_with_two_transactions_and_difference_categories()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        var firstCategory = Category.Create("Category");
        var secondCategory = Category.Create("Category 2");

        var transactions = new Transaction[]
        {
            new(5, Description.Create("Test"), firstCategory),
            new(5, Description.Create("Test 2"), secondCategory)
        };
        
        var account = GenerateFinancialAccount(accountId, transactions);

        var accountRepositoryMock = GenerateAccountRepositoryMock(account);
        
        var sut = new UseCases.GetCategoriesByAccount.GetCategoriesByAccount(accountRepositoryMock.Object);
        var output = new GetCategoryByAccountOutputTest();
        
        // Act
        sut.Execute(accountId, account.Owner.SubId, output);

        // AssertZ~A`a 
        Assert.Equal(2, output.Categories.Length);
    }

    private static FinancialAccount GenerateFinancialAccount(string accountId, IEnumerable<Transaction> transactions)
    {
        var account = new FinancialAccount(
            accountId,
            AccountName.Create("Test"),
            new Owner("Test", "Test"),
            10,
            TimeStamp.CreateNow(),
            transactions
        );
        return account;
    }

    private static Mock<IAccountQueriesRepository> GenerateAccountRepositoryMock(FinancialAccount account)
    {
        var accountRepositoryMock = new Mock<IAccountQueriesRepository>();
        accountRepositoryMock.Setup(x => x.GetCategoriesByAccount(account.Id, account.Owner.SubId))
            .Returns(() => account.Transactions.Items.Select(x => x.Category).Distinct().ToArray());
        
        return accountRepositoryMock;
    }
}

/*
* Todo:
* x Try to get from unexciting account.
* x Try to get with invalid Account Id format.
* x Get from account without transactions.
* x Get from account with transactions with same category successfully.
* - 
*/