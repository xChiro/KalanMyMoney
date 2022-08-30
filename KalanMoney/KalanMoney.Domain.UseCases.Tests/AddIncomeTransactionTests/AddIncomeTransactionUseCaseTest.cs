using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.AddIncomeTransactionTests;

public class AddIncomeTransactionUseCaseTest
{
    [Fact]
    public void Try_to_add_an_income_transaction_to_an_unexciting_account()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountQueriesRepository>();

        accountRepositoryMock.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialAccount));

        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        
        var sut = new AddIncomeTransactionUseCase(accountRepositoryMock.Object, accountCommandsRepository.Object);
        var accountId = Guid.NewGuid().ToString();
        
        var inputRequest = new AddTransactionRequest(accountId, 1500.00m, "Test", "Salary");
        
        var outPut = new AddIncomeTransactionOutputMock();

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(inputRequest, outPut));
    }

    [Fact]
    public void Add_a_income_transaction_to_an_existing_account_successfully()
    {
        // Arrange
        const decimal accountBalance = 104.5m;
        const decimal transactionAmount = 10m;
        const decimal expectedAccountBalance = accountBalance + transactionAmount;

        var owner = new Owner("test","test");
        const string transactionDescription = "Transaction Description";
        const string category = "Salary";
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription, category);

        var accountCommandsRepository = new AccountCommandsRepositoryMock();

        var accountQueryRepository = SetupAccountQueryRepositoryMock(financialAccount);
        var sut = new AddIncomeTransactionUseCase(accountQueryRepository.Object, accountCommandsRepository);
        var transaction = new AddTransactionRequest(financialAccount.Id, transactionAmount, transactionDescription, category);
        var outputMock = new AddIncomeTransactionOutputMock();
        
        // Act
        sut.Execute(transaction, outputMock);
        
        // Assert
        Assert.Equal(expectedAccountBalance, outputMock.AccountBalance);
        Assert.Equal(expectedAccountBalance, accountCommandsRepository.Balance.Amount);
        Assert.Equal(transactionAmount, accountCommandsRepository.ResultTransaction.Amount);
        Assert.Equal(financialAccount.Id, accountCommandsRepository.AccountId);
    }
    
    [Theory]
    [InlineData(100.00, 10.0, 110)]
    [InlineData(100.00, -10.0, 110)]
    public void Add_a_new_income_transaction_to_an_account_and_check_persistence_successfully(decimal accountBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        const string transactionDescription = "Test Description";
        const string category = "Salary";
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription, category);
        var accountQueriesRepository = SetupAccountQueryRepositoryMock(financialAccount);

        var accountCommandRepository = new AccountCommandsRepositoryMock();
        var sut = new AddIncomeTransactionUseCase(accountQueriesRepository.Object,
            accountCommandRepository);

        var output = new AddIncomeTransactionOutputMock();
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, transactionAmount, 
            transactionDescription, category);
        
        // Act
        sut.Execute(addTransactionRequest, output);

        // Assert
        Assert.NotNull(output.TransactionId);
        Assert.Equal(Math.Abs(transactionAmount), accountCommandRepository.ResultTransaction.Amount);
        Assert.Equal(Description.Create(transactionDescription), accountCommandRepository.ResultTransaction.Description);
        Assert.Equal(expectedBalance, output.AccountBalance);
    }
    
    private static FinancialAccount CreateFinancialAccount(decimal accountBalance, Owner owner, string transactionDescription, string category)
    {
        var currentTransactions = new Transaction[]
        {
            new(Guid.NewGuid().ToString(), accountBalance, Description.Create(transactionDescription), Category.Create(category), DateTime.UtcNow)
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            accountBalance, DateTime.UtcNow, currentTransactions);   
        
        return financialAccount;
    }

    private static Mock<IAccountQueriesRepository> SetupAccountQueryRepositoryMock(FinancialAccount financialAccount)
    {
        var accountQueryRepository = new Mock<IAccountQueriesRepository>();
        
        accountQueryRepository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        
        return accountQueryRepository;
    }
}