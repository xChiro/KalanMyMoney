using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.CreateAccount;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Tests.Adapters;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests;

public class CreateAccountUseCaseTest
{
    [Fact]
    public void Create_a_new_account_with_a_default_category_successfully()
    {
        // Arrange 
        const decimal openingTransaction = 0;
        
        var createAccountRequest = CreateAccountRequest("Normal Name");
        var accountRepositoryMock = AccountRepositoryMock();
        
        var createAccountOutput = new CreateAccountOutputMock();
        var sut = new CreateAccountUseCase(accountRepositoryMock.Object);

        // Act
        sut.Execute(createAccountRequest, createAccountOutput);

        // Assert
        Assert.True(!string.IsNullOrEmpty(createAccountOutput.AccountId) && createAccountOutput.AccountBalance == openingTransaction);
    }
    
    [Fact]
    public void Try_to_create_a_new_category_but_the_name_is_to_long()
    {
        // Arrange
        var accountRepositoryMock = AccountRepositoryMock();

        var createAccountOutput = new CreateAccountOutputMock();
        var sut = new CreateAccountUseCase(accountRepositoryMock.Object);
        
        const string longName =
            "This its a very very very long accountName and we follow TDD and Clean Architecture principles.";
        
        var createAccountRequest = CreateAccountRequest(longName);
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => sut.Execute(createAccountRequest, createAccountOutput));
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

    private static Mock<IAccountCommandsRepository> AccountRepositoryMock()
    {
        var accountRepositoryMock = new Mock<IAccountCommandsRepository>();
        accountRepositoryMock.Setup(x => x.OpenAccount(It.IsAny<FinancialAccount>()));
        return accountRepositoryMock;
    }
}