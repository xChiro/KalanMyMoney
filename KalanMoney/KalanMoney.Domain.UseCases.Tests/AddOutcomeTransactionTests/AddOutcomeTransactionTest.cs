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
        accountQueriesRepository.Setup(repository => repository.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialAccount));
        
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();

        var accountCommandRepository = new Mock<IAccountCommandsRepository>();
        
        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object, accountCommandRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1005.2m,"Test");

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(addTransactionRequest, new AddOutcomeTransactionOutput()));
    }

    [Fact]
    public void Try_to_add_an_outcome_transaction_to_unexciting_category()
    {
        // Arrange
        const decimal baseTransaction = 1005.4m;
        const string transactionDescription = "Transaction Description";
        
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        var financialAccount = CreateFinancialAccount(baseTransaction, owner, transactionDescription);

        var accountQueriesRepository = CreateAccountQueriesRepositoryMock(financialAccount);

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository.Setup(rep => rep.GetCategoryById(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialCategory));
        
        var accountCommandRepository = new Mock<IAccountCommandsRepository>();

        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object, accountCommandRepository.Object);
        var addTransactionRequest = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100.5m, "Test");

        // Act/Assert
        Assert.Throws<CategoryNotFoundException>(() => sut.Execute(addTransactionRequest, new AddOutcomeTransactionOutput()));
    }

    [Theory]
    [InlineData(100.00, 10.0, 90)]
    [InlineData(100.00, -10.0, 90)]
    public void Add_a_new_output_transaction_to_an_existing_account_successfully(decimal accountBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        const string transactionDescription = "Transaction Description";
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription);

        var financialCategory = CreateFinancialCategory(financialAccount, owner, accountBalance, transactionDescription);

        var accountCommandRepository = new Mock<IAccountCommandsRepository>();
        accountCommandRepository.Setup(x =>
            x.AddTransaction(It.IsAny<AddTransactionModel>(), It.IsAny<Transaction>()));
        
        var categoryQueriesRepository = CategoryQueriesRepositoryMockSetup(financialCategory);
        var accountQueriesRepository = AccountQueriesRepositoryMockSetup(financialAccount);
        
        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object,
            accountCommandRepository.Object);
        
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, financialCategory.Id, transactionAmount, "Test");
        var output = new AddOutcomeTransactionOutput();

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
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription);
        var financialCategory = CreateFinancialCategory(financialAccount, owner, accountBalance, transactionDescription);
        
        var categoryQueriesRepository = CategoryQueriesRepositoryMockSetup(financialCategory);
        var accountQueriesRepository = AccountQueriesRepositoryMockSetup(financialAccount);

        var accountCommandRepository = new AccountCommandsRepositoryMock(financialAccount);
        var sut = new AddOutcomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object,
            accountCommandRepository);

        var output = new AddOutcomeTransactionOutput();
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, financialCategory.Id, transactionAmount, transactionDescription);
        // Act
        sut.Execute(addTransactionRequest, output);

        // Assert
        Assert.Equal( -Math.Abs(transactionAmount), accountCommandRepository.ResultTransaction.Amount);
        Assert.Equal(new Balance(expectedBalance), accountCommandRepository.ResultModel.CategoryBalance);
        Assert.Equal(Description.Create(transactionDescription), accountCommandRepository.ResultTransaction.Description);
        Assert.Equal(expectedBalance, output.AccountBalance);
    }

    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepositoryMock(FinancialAccount financialAccount)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository
            .Setup(repository => repository.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        return accountQueriesRepository;
    }

    private static Mock<IAccountQueriesRepository> AccountQueriesRepositoryMockSetup(FinancialAccount financialAccount)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(repository => repository.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        return accountQueriesRepository;
    }

    private static Mock<ICategoryQueriesRepository> CategoryQueriesRepositoryMockSetup(FinancialCategory financialCategory)
    {
        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueriesRepository.Setup(rep => rep.GetCategoryById(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialCategory);
        return categoryQueriesRepository;
    }

    private static FinancialCategory CreateFinancialCategory(FinancialAccount financialAccount, Owner owner,
        decimal accountBalance, string transactionDescription)
    {
        var financialCategory = new FinancialCategory(Guid.NewGuid().ToString(), AccountName.Create("Test"),
            financialAccount.Id, owner, accountBalance, new Transaction[1]
            {
                new (new Guid().ToString(), accountBalance, Description.Create(transactionDescription), TimeStamp.CreateNow())
            });
        
        return financialCategory;
    }

    private static FinancialAccount CreateFinancialAccount(decimal baseTransaction, Owner owner, string transactionDescription)
    {
        var transactions = new Transaction[1]
        {
            new(Guid.NewGuid().ToString(), baseTransaction, Description.Create(transactionDescription), TimeStamp.CreateNow())
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            baseTransaction, TimeStamp.CreateNow(), transactions);
        
        return financialAccount;
    }
}