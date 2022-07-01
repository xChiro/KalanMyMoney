using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.AccountDashboardTests;

public class AccountDashboardUseCaseTest
{
    [Fact]
    public void Try_to_request_a_dashboard_of_unexciting_account()
    {
        // Arrange

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x =>
                x.GetTransactions(It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Returns(default(Transaction[]));
        
        var sut = new AccountDashboardUseCase(accountQueriesRepository.Object);

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(Guid.NewGuid().ToString(), new AccountDashboardOutputMock()));
        
    }
}