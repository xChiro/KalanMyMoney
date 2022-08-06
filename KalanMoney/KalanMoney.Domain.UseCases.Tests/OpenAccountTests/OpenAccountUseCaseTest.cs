using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
using Moq;
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
    public void Open_an_account_successfully()
    {
        // Arrange 
        const decimal openingTransaction = 0;

        var createAccountRequest = CreateAccountRequest("Normal Name");
        var accountRepositoryMock = new AccountCommandsRepositoryMock();

        var createAccountOutput = new OpenAccountOutputMock();
        var sut = new OpenAccountUseCase(accountRepositoryMock);

        // Act
        sut.Execute(createAccountRequest, createAccountOutput);

        // Assert
        Assert.True(!string.IsNullOrEmpty(createAccountOutput.AccountId) &&
                    createAccountOutput.AccountBalance == openingTransaction);
        Assert.NotNull(accountRepositoryMock.FinancialAccount);
        Assert.Equal(new Balance(0), accountRepositoryMock.FinancialAccount.Balance);
    }

    private static CreateAccountRequest CreateAccountRequest(string accountName)
    {
        var createAccountRequest = new CreateAccountRequest(
            Guid.NewGuid().ToString(),
            "Owner Test",
            accountName
        );

        return createAccountRequest;
    }
}
