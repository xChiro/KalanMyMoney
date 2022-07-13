using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.CreateCategory;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
using Moq;
using Xunit;

namespace KalanMoney.Domain.UseCases.Tests.CreateCategoryTests;

public class CreateCategoryUseCaseTest
{
    [Fact]
    public void Submit_request_to_create_a_category_return_categoryId_in_output_successfully()
    {
        // Arrange
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        var accountQueriesRepository = AccountQueryGetAccountOnlySetup("OwnerTest");

        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var output = new CreateCategoryOutputMock();
        
        const string categoryName = "Salary";
        var request = CreateCategoryRequest(categoryName, Guid.NewGuid().ToString());
        
        // Act
        sut.Execute(request, output);

        // Assert
        Assert.NotEmpty(output.CategoryId);
    }

    [Fact]
    public void Try_to_create_a_category_with_a_long_name()
    {
        // Arrange
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        var accountQueriesRepository = AccountQueryGetAccountOnlySetup("OwnerTest");
        
        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var output = new CreateCategoryOutputMock();
        
        const string categoryName = "This its a very very long name for a category and its invalid.";
        var request = CreateCategoryRequest(categoryName, Guid.NewGuid().ToString());
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => sut.Execute(request, output));
    }

    [Fact]
    public void Try_to_create_a_category_in_an_unexciting_account()
    {
        // Arrange 
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x => x.GetAccountOnly(It.IsAny<string>()))
            .Returns(default(FinancialAccount));
        
        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var output = new CreateCategoryOutputMock();
        var request = CreateCategoryRequest("Salary", Guid.NewGuid().ToString());

        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(request, output));
    }

    [Fact]
    public void Create_a_category_in_account_and_persist_category_successfully()
    {
        // Arrange 
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        const string ownerName = "OwnerTest";
        var accountQueriesRepository = AccountQueryGetAccountOnlySetup(ownerName);

        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var output = new CreateCategoryOutputMock();
        
        const string categoryName = "Salary";
        var accountId = Guid.NewGuid().ToString();
        var request = CreateCategoryRequest(categoryName, accountId);
        
        // Act
        sut.Execute(request, output);
        
        // Assert
        Assert.NotNull(output.CategoryId);
        Assert.Single(categoryCommandsRepository.Categories);
        Assert.Equal(categoryName, categoryCommandsRepository.Categories.First().Value.Name.Value);
        Assert.Equal(accountId, categoryCommandsRepository.Categories.First().Value.AccountId);
        Assert.Equal(output.CategoryId, categoryCommandsRepository.Categories.First().Value.Id);
        Assert.NotNull(categoryCommandsRepository.Categories.First().Value.Owner);
        Assert.Equal(ownerName, categoryCommandsRepository.Categories.First().Value.Owner.Name);
    }

    private static CreateCategoryRequest CreateCategoryRequest(string categoryName, string accountId)
    {
        var request = new CreateCategoryRequest(accountId, categoryName);
        return request;
    }

    private static Mock<IAccountQueriesRepository> AccountQueryGetAccountOnlySetup(string ownerName)
    {
        var financialCategory = new FinancialAccount(AccountName.Create("Account Test"), 
            Guid.NewGuid().ToString(), ownerName);

        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x => x.GetAccountOnly(It.IsAny<string>()))
            .Returns(financialCategory);
        return accountQueriesRepository;
    }
}