using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.AddOutcomeTransactionTests;

public class AddOutcomeTransactionTest
{
    [Fact]
    public void Try_to_add_an_outcome_transaction_to_unexciting_account()
    {
        // Arrange
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccountById(It.IsAny<string>()))
            .Returns(default(FinancialAccount));
        
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1005.2m);

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(addTransactionRequest));
    }

    [Fact]
    public void Try_to_add_an_outcome_transaction_to_unexciting_category()
    {
        // Arrange
        const decimal baseTransaction = 1005.4m;
        var financialAccount = CreateFinancialAccount(baseTransaction);

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccountById(It.IsAny<string>()))
            .Returns(financialAccount);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository.Setup(rep => rep.GetCategoryById(It.IsAny<string>()))
            .Returns(default(FinancialCategory));

        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100.5m);

        // Act/Assert
        Assert.Throws<CategoryNotFoundException>(() => sut.Execute(addTransactionRequest));
    }

    [Fact]
    public void Add_a_new_output_transaction_to_an_existing_account_with_positive_amount_successfully()
    {
        // Arrange
        const decimal accountBalance = 100.00m;
        const decimal transactionAmount = 10m;
        var expectedBalance = new Balance(accountBalance - transactionAmount);

        var financialAccount = CreateFinancialAccount(accountBalance);
        
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccountById(It.IsAny<string>()))
            .Returns(financialAccount);


        // Act

        // Assert
    }

    private static FinancialAccount CreateFinancialAccount(decimal baseTransaction)
    {
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        var balance = new Balance(baseTransaction);
        var transactions = new Transaction[1]
        {
            new(Guid.NewGuid().ToString(), baseTransaction, TimeStamp.CreateNow())
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            balance, TimeStamp.CreateNow(), transactions);
        return financialAccount;
    }
}