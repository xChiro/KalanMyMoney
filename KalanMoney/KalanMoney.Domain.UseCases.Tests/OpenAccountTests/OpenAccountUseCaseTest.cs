using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Repositories;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.OpenAccountTests;

public class OpenAccountUseCaseTest
{
    [Fact]
    public void Open_an_account_with_a_default_category_successfully()
    {
        // Arrange 
        const decimal openingTransaction = 0;
        
        var createAccountRequest = CreateAccountRequest("Normal Name");
        var accountRepositoryMock = GetSetupAccountRepositoryMock();
        
        var createAccountOutput = new OpenAccountOutputMock();
        var sut = new OpenAccountUseCase(accountRepositoryMock.Object);

        // Act
        sut.Execute(createAccountRequest, createAccountOutput);

        // Assert
        Assert.True(!string.IsNullOrEmpty(createAccountOutput.AccountId) && createAccountOutput.AccountBalance == openingTransaction);
    }
    
    [Fact]
    public void Try_to_create_a_new_category_but_the_name_is_to_long()
    {
        // Arrange
        var accountRepositoryMock = GetSetupAccountRepositoryMock();

        var createAccountOutput = new OpenAccountOutputMock();
        var sut = new OpenAccountUseCase(accountRepositoryMock.Object);
        
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

    private static Mock<IAccountCommandsRepository> GetSetupAccountRepositoryMock()
    {
        var accountRepositoryMock = new Mock<IAccountCommandsRepository>();
        accountRepositoryMock.Setup(x => x.OpenAccount(It.IsAny<FinancialAccount>()));
        
        return accountRepositoryMock;
    }
}