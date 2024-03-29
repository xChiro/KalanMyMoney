using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
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
        accountQueriesRepository.Setup(repository => repository.GetAccountWithoutTransactions(
                It.IsAny<string>(), It.IsAny<string>()))
            .Returns(default(FinancialAccount));
        
        var accountCommandRepository = new Mock<IAccountCommandsRepository>();
        
        var sut = new AddOutcomeTransaction.AddOutcomeTransaction(accountQueriesRepository.Object, accountCommandRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),1005.2m,
            "Test", "Salary");

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(addTransactionRequest, new AddOutcomeTransactionOutputMock()));
    }


    [Theory]
    [InlineData(100.00, 10.0, 90)]
    [InlineData(100.00, -10.0, 90)]
    public void Add_a_new_output_transaction_to_an_existing_account_successfully(decimal accountBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        const string transactionDescription = "Transaction Description";
        const string category = "Salary";
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription, category);

        var accountCommandRepository = new Mock<IAccountCommandsRepository>();
        accountCommandRepository.Setup(x =>
            x.StoreTransaction(It.IsAny<string>(), It.IsAny<Balance>(), It.IsAny<Transaction>()));
        
        var accountQueriesRepository = AccountQueriesRepositoryMockSetup(financialAccount);
        
        var sut = new AddOutcomeTransaction.AddOutcomeTransaction(accountQueriesRepository.Object, accountCommandRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, owner.SubId, transactionAmount, 
            "Test", "Salary");
        
        var output = new AddOutcomeTransactionOutputMock();

        // Act
        sut.Execute(addTransactionRequest, output);

        // Assert
        Assert.NotNull(output.TransactionId);
        Assert.Equal(expectedBalance, output.AccountBalance);
    }

    [Theory]
    [InlineData(100.00, 10.0, 90)]
    [InlineData(100.00, -10.0, 90)]
    public void Add_a_new_output_transaction_to_an_account_and_check_persistence_successfully(decimal accountBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        const string transactionDescription = "Transaction Description";
        const string category = "Salary";

        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription, category);
        var accountQueriesRepository = AccountQueriesRepositoryMockSetup(financialAccount);

        var accountCommandRepository = new AccountCommandsRepositoryMock();
        var sut = new AddOutcomeTransaction.AddOutcomeTransaction(accountQueriesRepository.Object, accountCommandRepository);

        var output = new AddOutcomeTransactionOutputMock();
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, owner.SubId, transactionAmount, 
            transactionDescription, category);
        // Act
        sut.Execute(addTransactionRequest, output);

        // Assert
        Assert.Equal( -Math.Abs(transactionAmount), accountCommandRepository.ResultTransaction.Amount);
        Assert.Equal(Description.Create(transactionDescription), accountCommandRepository.ResultTransaction.Description);
        Assert.Equal(expectedBalance, output.AccountBalance);
    }

    private static Mock<IAccountQueriesRepository> AccountQueriesRepositoryMockSetup(FinancialAccount financialAccount)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccountWithoutTransactions(
                It.IsAny<string>(), It.IsAny<string>()))
            .Returns(financialAccount);
        return accountQueriesRepository;
    }

    private static FinancialAccount CreateFinancialAccount(decimal baseTransaction, Owner owner, string transactionDescription, string category)
    {
        var transactions = new Transaction[1]
        {
            new(Guid.NewGuid().ToString(), baseTransaction, Description.Create(transactionDescription), 
                Category.Create(category), TimeStamp.CreateNow())
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            baseTransaction, TimeStamp.CreateNow(), transactions);
        
        return financialAccount;
    }
}