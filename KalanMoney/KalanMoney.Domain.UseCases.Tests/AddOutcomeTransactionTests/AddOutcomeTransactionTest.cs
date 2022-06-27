using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.AddOutcomeTransactionTests;

public class AddOutcomeTransactionTest
{
    [Fact]
    public void Try_to_add_a_outcome_transaction_to_unexciting_account()
    {
        // Arrange
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccountById(It.IsAny<string>()))
            .Returns(default(FinancialAccount));

        // var sut = new AddOutcomeTransaction();

        // Act

        // Assert
    } 
}