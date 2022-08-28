using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.OpenAccountTests;

public class OpenAccountUseCaseTest
{
    [Fact]
    public void Try_to_open_an_account_but_the_name_is_to_long()
    {
        // Arrange
        var accountRepositoryMock = new AccountCommandsRepositoryMock();
        var createAccountRequest =
            CreateAccountRequest("Very very very very very loooooonnnnnggggggg name, its invalid.");
        
        var sut = new OpenAccountUseCase(accountRepositoryMock);
        var openAccountOutputMock = new OpenAccountOutputMock();

        // Act/Assert
        Assert.Throws<AccountNameException>(() => sut.Execute(createAccountRequest, openAccountOutputMock));
    }

    [Fact]
    public void Open_an_account_with_account_name_successfully()
    {
        // Arrange 
        const string accountName = "Normal Name";
        var createAccountRequest = CreateAccountRequest(accountName);
        var accountRepositoryMock = new AccountCommandsRepositoryMock();

        var createAccountOutput = new OpenAccountOutputMock();
        var sut = new OpenAccountUseCase(accountRepositoryMock);

        // Act
        sut.Execute(createAccountRequest, createAccountOutput);

        // Assert
        Assert.NotNull(createAccountOutput.AccountId);
        Assert.NotEmpty(createAccountOutput.AccountId);
        Assert.Equal(0, createAccountOutput.AccountBalance);
        Assert.NotNull(accountRepositoryMock.FinancialAccount);
        Assert.Equal(new Balance(0), accountRepositoryMock.FinancialAccount.Balance);
        Assert.Equal( accountName, accountRepositoryMock.FinancialAccount.Name.Value);
    }

    [Fact]
    public void Open_an_account_without_name_set_default_name_instead_successfully()
    {
        // Arrange
        var accountRepositoryMock = new AccountCommandsRepositoryMock();
        var sut = new OpenAccountUseCase(accountRepositoryMock);
        
        var createAccountRequest = CreateAccountRequest();
        var output = new OpenAccountOutputMock();

        // Act
        sut.Execute(createAccountRequest, output);

        // Assert
        Assert.NotNull(output.AccountId);
        Assert.NotEmpty(output.AccountId);
        Assert.Equal(0, output.AccountBalance);
        Assert.NotNull(accountRepositoryMock.FinancialAccount);
        Assert.Equal(new Balance(0), accountRepositoryMock.FinancialAccount.Balance);
        Assert.Equal($"{createAccountRequest.OwnerName} Account", accountRepositoryMock.FinancialAccount.Name.Value);
    }
    
    private static CreateAccountRequest CreateAccountRequest(string? accountName = null)
    {
        var createAccountRequest = new CreateAccountRequest(
            Guid.NewGuid().ToString(),
            "Owner Test",
            accountName
        );

        return createAccountRequest;
    }
}
