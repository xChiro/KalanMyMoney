using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.ValueObjects;
using Xunit;

namespace KalanMoney.Domain.Entities.Tests.ValueObjectsTests;

public class AccountNameTest
{
    [Fact]
    public void Try_to_create_an_new_account_name_but_the_name_is_to_long()
    {
        // Arrange
        const string longName 
            = "This its a very very very long accountName and we follow TDD and Clean Architecture principles.";
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => AccountName.Create(longName));
    }

    [Fact]
    public void Try_to_create_an_account_name_with_the_alternative_name_but_its_to_long()
    {
        // Arrange
        const string longName 
            = "This its a very very very long accountName and we follow TDD and Clean Architecture principles.";
        
        var empty = string.Empty;
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => AccountName.Create(empty, longName));
    }

    [Fact]
    public void Try_to_create_an_account_name_but_the_name_and_alternative_are_empties()
    {
        // Arrange
        var empty = string.Empty;
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => AccountName.Create(empty, empty));
    }
    
    [Fact]
    public void Create_an_account_name_with_empty_name_chose_alternative_instead_successfully()
    {
        // Arrange
        const string alternativeName = "Alternative Name";
        var empty = string.Empty;

        // Act
        var sut = AccountName.Create(empty, alternativeName);

        // Assert
        Assert.Equivalent(alternativeName, sut.Value);
    }

    [Fact]
    public void Create_an_account_name_successfully()
    {
        // Arrange
        const string name = "Account Name";
        
        // Assert
        var sut = AccountName.Create(name);

        // Act
        Assert.Equivalent(name, sut.Value);
    }
}