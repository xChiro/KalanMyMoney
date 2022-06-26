using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
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

        accountRepositoryMock.Setup(x => x.GetAccountById(It.IsAny<string>()))
            .Returns(default(FinancialAccount));

        var categoryQueriesRepository = new Mock<ICategoryQueriesRepository>();
        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        
        var sut = new AddIncomeTransactionUseCase(accountRepositoryMock.Object, categoryQueriesRepository.Object, accountCommandsRepository.Object);
        var accountId = Guid.NewGuid().ToString();
        var categoryId = Guid.NewGuid().ToString();
        
        var inputRequest = new AddIncomeTransactionRequest(accountId, categoryId, 1500.00m);
        
        var outPut = new AddIncomeTransactionOutputMock();

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(inputRequest, outPut));
    }

    [Fact]
    public void Try_to_add_an_income_transaction_with_an_unexciting_category()
    {
        // Arrange
        var categoryQueryRepositoryMock = new Mock<ICategoryQueriesRepository>();
        categoryQueryRepositoryMock.Setup(x => x.GetCategoryById(It.IsAny<string>()))
            .Returns(default(FinancialCategory));
        
        var accountQueriesRepositoryMock = new Mock<IAccountQueriesRepository>();
        accountQueriesRepositoryMock.Setup(x => x.GetAccountById(It.IsAny<string>()))
            .Returns(new FinancialAccount(AccountName.Create("Test"), "Owner Id", "Test Name"));
        
        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();

        var sut = new AddIncomeTransactionUseCase(accountQueriesRepositoryMock.Object, categoryQueryRepositoryMock.Object, accountCommandsRepository.Object);
        var request = new AddIncomeTransactionRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 145.23m);
        
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

        var financialAccount = CreateFinancialAccount(accountBalance, owner);
        var accountQueryRepository = SetupAccountQueryRepositoryMock(financialAccount);

        var accountCommandsRepository = new Mock<IAccountCommandsRepository>();
        accountCommandsRepository.Setup(x => x.OpenAccount(It.IsAny<FinancialAccount>()));

        var sut = new AddIncomeTransactionUseCase(accountQueryRepository.Object, categoryQueryRepository.Object, accountCommandsRepository.Object);
        var transaction = new AddIncomeTransactionRequest( financialAccount.Id, financialCategory.Id, transactionAmount);
        var outputMock = new AddIncomeTransactionOutputMock();
        
        // Act
        sut.Execute(transaction, outputMock);
        
        // Assert
        Assert.Equal(expectedAccountBalance, outputMock.AccountBalance);
        Assert.Equal(transactionAmount, outputMock.CategoryBalance);
    }
    
    private static FinancialAccount CreateFinancialAccount(decimal accountBalance, Owner owner)
    {
        var balance = new Balance(accountBalance);
        var currentTransactions = new Transaction[1]
        {
            new(Guid.NewGuid().ToString(), accountBalance, TimeStamp.CreateNow())
        };

        var financialAccount = new FinancialAccount(Guid.NewGuid().ToString(), AccountName.Create("Test"), owner,
            balance, TimeStamp.CreateNow(), currentTransactions);
        
        return financialAccount;
    }

    private static Mock<IAccountQueriesRepository> SetupAccountQueryRepositoryMock(FinancialAccount financialAccount)
    {
        var accountQueryRepository = new Mock<IAccountQueriesRepository>();
        
        accountQueryRepository.Setup(x => x.GetAccountById(It.IsAny<string>()))
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
        categoryQueryRepository.Setup(x => x.GetCategoryById(It.IsAny<string>()))
            .Returns(financialCategory);
        return categoryQueryRepository;
    }
}