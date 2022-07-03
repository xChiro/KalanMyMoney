using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.CreateCategory;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
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
        var accountQueriesRepository = CreateAccountQueriesRepository(true);

        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);

        const string categoryName = "Salary";
        var accountId = Guid.NewGuid().ToString();
        var output = new CreateCategoryOutputMock();
        
        // Act
        sut.Execute(new CreateCategoryRequest(accountId, categoryName), output);

        // Assert
        Assert.NotEmpty(output.CategoryId);
    }

    [Fact]
    public void Try_to_create_a_category_with_a_long_name()
    {
        // Arrange
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        var accountQueriesRepository = CreateAccountQueriesRepository(true);
        
        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var accountId = Guid.NewGuid().ToString();
        var output = new CreateCategoryOutputMock();
        const string categoryName = "This its a very very long name for a category and its invalid.";
        
        // Act/Assert
        Assert.Throws<AccountNameException>(() => sut.Execute(new CreateCategoryRequest(accountId, categoryName), output));
    }

    [Fact]
    public void Try_to_create_a_category_in_an_unexciting_account()
    {
        // Arrange 
        var categoryCommandsRepository = new CategoryCommandsRepositoryMock();
        var accountQueriesRepository = CreateAccountQueriesRepository(false);
        
        var sut = new CreateCategoryUseCase(categoryCommandsRepository, accountQueriesRepository.Object);
        var output = new CreateCategoryOutputMock();
        const string categoryName = "Salary";
        var accountId = Guid.NewGuid().ToString();
        
        // Act/Assert
        Assert.Throws<AccountNotFoundException>(() => sut.Execute(new CreateCategoryRequest(accountId, categoryName), output));
    }
    
    private static Mock<IAccountQueriesRepository> CreateAccountQueriesRepository(bool existAccount)
    {
        var accountQueriesRepository = new Mock<IAccountQueriesRepository>();
        accountQueriesRepository.Setup(x => x.AccountExists(It.IsAny<string>()))
            .Returns(existAccount);
        return accountQueriesRepository;
    }
}