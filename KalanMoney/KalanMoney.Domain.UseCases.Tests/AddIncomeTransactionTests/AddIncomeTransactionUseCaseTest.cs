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

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        
        var sut = new AddIncomeTransactionUseCase(accountRepositoryMock.Object, categoryQueriesRepository.Object, accountCommandsRepository.Object);
        var accountId = Guid.NewGuid().ToString();
        var categoryId = Guid.NewGuid().ToString();
        
        var inputRequest = new AddTransactionRequest(accountId, categoryId, 1500.00m, "Test");
        
        var outPut = new AddIncomeTransactionOutputMock();

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(inputRequest, outPut));
    }

    [Fact]
    public void Try_to_add_an_income_transaction_with_an_unexciting_category()
    {
        // Arrange
        var categoryQueryRepositoryMock = new Mock<ICategoryQueriesRepository>();
        categoryQueryRepositoryMock.Setup(x => x.GetCategoryById(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(default(FinancialCategory));
        
        var accountQueriesRepositoryMock = new Mock<IAccountQueriesRepository>();
        accountQueriesRepositoryMock.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(new FinancialAccount(AccountName.Create("Test"), "Owner Id", "Test Name"));
        
        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        var sut = new AddIncomeTransactionUseCase(accountQueriesRepositoryMock.Object, categoryQueryRepositoryMock.Object, accountCommandsRepository.Object);
        var request = new AddTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 145.23m, "Test");
        
        // Act/Assert
        Assert.Throws<CategoryNotFoundException>(() => sut.Execute(request, new AddIncomeTransactionOutputMock()));
    }

    [Fact]
    public void Add_a_income_transaction_to_an_existing_account_successfully()
    {
        // Arrange
        const decimal accountBalance = 104.5m;
        const decimal transactionAmount = 10m;
        const decimal expectedAccountBalance = accountBalance + transactionAmount;

        var owner = new Owner("test","test");
        
        var financialCategory = CreateNewFinancialCategory(owner);
        var categoryQueryRepository = SetupCategoryQueryRepositoryMock(financialCategory);

        const string transactionDescription = "Transaction Description";
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription);

        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        accountCommandsRepository.Setup(x => x.OpenAccount(It.IsAny<FinancialAccount>()));

        var accountQueryRepository = SetupAccountQueryRepositoryMock(financialAccount);
        var sut = new AddIncomeTransactionUseCase(accountQueryRepository.Object, categoryQueryRepository.Object, accountCommandsRepository.Object);
        var transaction = new AddTransactionRequest( financialAccount.Id, financialCategory.Id, transactionAmount, transactionDescription);
        var outputMock = new AddIncomeTransactionOutputMock();
        
        // Act
        sut.Execute(transaction, outputMock);
        
        // Assert
        Assert.Equal(expectedAccountBalance, outputMock.AccountBalance);
        Assert.Equal(transactionAmount, outputMock.CategoryBalance);
    }
    
    [Theory]
    [InlineData(100.00, 10.0, 110)]
    [InlineData(100.00, -10.0, 110)]
    public void Add_a_new_income_transaction_to_an_account_and_check_persistence_successfully(decimal accountBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var owner = new Owner(Guid.NewGuid().ToString(), "Test");
        const string transactionDescription = "Test Description";
        
        var financialAccount = CreateFinancialAccount(accountBalance, owner, transactionDescription);
        var financialCategory = CreateFinancialCategory(financialAccount.Id, owner, accountBalance, transactionDescription);
        
        var categoryQueriesRepository = SetupCategoryQueryRepositoryMock(financialCategory);
        var accountQueriesRepository = SetupAccountQueryRepositoryMock(financialAccount);

        var accountCommandRepository = new AccountCommandsRepositoryMock(financialAccount);
        var sut = new AddIncomeTransactionUseCase(accountQueriesRepository.Object, categoryQueriesRepository.Object,
            accountCommandRepository);

        var output = new AddIncomeTransactionOutputMock();
        var addTransactionRequest = new AddTransactionRequest(financialAccount.Id, financialCategory.Id, transactionAmount,transactionDescription);
        
        // Act
        sut.Execute(addTransactionRequest, output);

        // Assert
        Assert.NotNull(output.TransactionId);
        Assert.Equal(Math.Abs(transactionAmount), accountCommandRepository.ResultTransaction.Amount);
        Assert.Equal(Description.Create(transactionDescription), accountCommandRepository.ResultTransaction.Description);
        Assert.Equal(new Balance(expectedBalance), accountCommandRepository.ResultModel.CategoryBalance);
        Assert.Equal(expectedBalance, output.AccountBalance);
    }
    
    private static FinancialCategory CreateFinancialCategory(string financialAccountId, Owner owner,
        decimal accountBalance, string transactionDescription)
    {
        var financialCategory = new FinancialCategory(Guid.NewGuid().ToString(), AccountName.Create("Test"),
            financialAccountId, owner, accountBalance, new Transaction[1]
            {
                new (new Guid().ToString(), accountBalance, Description.Create(transactionDescription), TimeStamp.CreateNow())
            });
        
        return financialCategory;
    }

    private static FinancialAccount CreateFinancialAccount(decimal accountBalance, Owner owner, string transactionDescription)
    {
        var currentTransactions = new Transaction[1]
        {
            new(Guid.NewGuid().ToString(), accountBalance, Description.Create(transactionDescription), TimeStamp.CreateNow())
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            accountBalance, TimeStamp.CreateNow(), currentTransactions);   
        
        return financialAccount;
    }

    private static Mock<IAccountQueriesRepository> SetupAccountQueryRepositoryMock(FinancialAccount financialAccount)
    {
        var accountQueryRepository = new Mock<IAccountQueriesRepository>();
        
        accountQueryRepository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialAccount);
        
        return accountQueryRepository;
    }

    private static FinancialCategory CreateNewFinancialCategory(Owner owner)
    {
        var financialCategory = new FinancialCategory(AccountName.Create("Test"), Guid.NewGuid().ToString(),
            owner);
        return financialCategory;
    }

    private static Mock<ICategoryQueriesRepository> SetupCategoryQueryRepositoryMock(FinancialCategory financialCategory)
    {
        var categoryQueryRepository = new Mock<ICategoryQueriesRepository>();
        categoryQueryRepository.Setup(x => x.GetCategoryById(It.IsAny<string>(), It.IsAny<TransactionFilter>()))
            .Returns(financialCategory);
        return categoryQueryRepository;
    }
}