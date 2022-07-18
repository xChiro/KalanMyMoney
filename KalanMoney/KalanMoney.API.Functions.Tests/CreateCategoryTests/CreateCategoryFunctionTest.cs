using System.Text;
using KalanMoney.API.Functions.CreateCategory;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.CreateCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.CreateCategoryTests;

public class CreateCategoryFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_create_a_new_category_in_an_unexciting_account_return_not_found()
    {
        // Arrange 
        var accountNotFoundException = new AccountNotFoundException();
        var accountQueriesRepository = SetupCreateCategoryInputMock(accountNotFoundException);
        
        var sut = new CreateCategoryRequestFunction(accountQueriesRepository.Object);
        
        var requestBody = CreateRequestBodyJson("Test", Guid.NewGuid().ToString());
        var defaultHttpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("This name its to long for a category, so do not persist it.")]
    [InlineData("")]
    [InlineData(null)]
    public async void Try_to_create_a_new_category_with_an_invalid_name_return_bad_request(string categoryName)
    {
        // Arrange
        var accountNameException = new AccountNameException(categoryName);
        var createCategoryInputMock = SetupCreateCategoryInputMock(accountNameException);

        var sut = new CreateCategoryRequestFunction(createCategoryInputMock.Object);
        var body = CreateRequestBodyJson(categoryName, Guid.NewGuid().ToString());
        var httpRequest = CreateHttpRequest(body);

        // Act
        var results = await sut.RunAsync(httpRequest, Mock.Of<ILogger>());

        // Assert
        Assert.IsType<BadRequestResult>(results);
    }

    [Fact]
    public async void Create_a_new_category_correctly_return_ok_result_successfully()
    {
        // Arrange
        var accountId = Guid.NewGuid().ToString();
        const string categoryName = "Category Test";
        
        var createCategoryInput = new Mock<ICreateCategoryInput>();

        var sut = new CreateCategoryRequestFunction(createCategoryInput.Object);
        var body = CreateRequestBodyJson(categoryName, accountId);
        var httpRequest = CreateHttpRequest(body);
        
        // Act
        var result = await sut.RunAsync(httpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    private static Mock<ICreateCategoryInput> SetupCreateCategoryInputMock(Exception accountNameException)
    {
        var accountQueriesRepository = new Mock<ICreateCategoryInput>();
        accountQueriesRepository.Setup(x => x.Execute(It.IsAny<CreateCategoryRequest>(), It.IsAny<ICreateCategoryOutput>()))
            .Throws(accountNameException);
        return accountQueriesRepository;
    }

    private static string CreateRequestBodyJson(string categoryName, string accountId)
    {
        var requestBody = $"{{ 'CategoryName': '{categoryName}', 'AccountId': '{accountId}' }}";
        
        return requestBody;
    }
}