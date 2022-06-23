using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.CreateAccount;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Test.Adapters;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Test;

public class CreateAccountUseCaseTest
{
    [Fact]
    public void Create_a_new_account_with_a_default_category_successfully()
    {
        // Arrange 
        const decimal openingTransaction = 0;
        var createAccountRequest = new CreateAccountRequest(
            Guid.NewGuid().ToString(),
            "Owner Test",
            "Account Test"
        );

        var accountRepositoryMock = new Mock<IAccountCommandsRepository>();
        accountRepositoryMock.Setup(x =>  x.OpenAccount(It.IsAny<FinancialAccount>()));

        var createAccountOutput = new CreateAccountOutputMock();
        var sut = new CreateAccountUseCase(accountRepositoryMock.Object);

        // Act
        sut.Execute(createAccountRequest, createAccountOutput);

        // Assert
        Assert.True(!string.IsNullOrEmpty(createAccountOutput.AccountId) && createAccountOutput.AccountBalance == openingTransaction);
    }
}