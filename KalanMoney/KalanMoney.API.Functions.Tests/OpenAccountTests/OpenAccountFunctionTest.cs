using System.Text;
using System.Web.Http;
using KalanMoney.API.Functions.OpenAccount;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.OpenAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.OpenAccountTests;

public class OpenAccountFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_open_an_account_with_invalid_json_property_return_bad_request()
    {
        // Arrange
        var accountInput = CreateOpenAccountInputException(new AccountNameException(string.Empty));

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "{ 'badRequest': 'Test' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<BadRequestErrorMessageResult>(result);
    }

    [Theory]
    [InlineData("The name of this accounts its to long to be permitted.")]
    [InlineData("")]
    [InlineData(null)]
    public async void Try_to_open_an_account_with_an_invalid_name_return_bad_request(string categoryName)
    {
        // Arrange
        var accountInput = CreateOpenAccountInputException(new AccountNameException(categoryName));
        var sut = new OpenAccountFunction(accountInput);
        
        var defaultHttpRequest = CreateHttpRequestCategoryName(categoryName);
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<BadRequestErrorMessageResult>(result);
    }

    [Fact]
    public async void Try_to_open_an_account_with_an_invalid_json_return_bad_request()
    {
        // Arrange
        var accountInput = CreateOpenAccountInputException(new Exception());

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "'Name': 'The name of this accounts its to long to be permitted.' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
        
    }

    [Fact]
    public async void Open_an_account_successfully_returns_ok_result()
    {
        // Arrange
        var accountInput = new Mock<IOpenAccountInput>();

        var sut = new OpenAccountFunction(accountInput.Object);
        
        const string requestBody = "{ 'Name': 'Test Account.' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        
    }

    private static IOpenAccountInput CreateOpenAccountInputException(Exception exception)
    {
        var openAccountInput = new Mock<IOpenAccountInput>();
        openAccountInput.Setup(x => x.Execute(It.IsAny<CreateAccountRequest>(), It.IsAny<IOpenAccountOutput>()))
            .Throws(exception);
        
        return openAccountInput.Object;
    }

    private static DefaultHttpRequest CreateHttpRequestCategoryName(string categoryName)
    {
        var requestBody = $"{{ 'Name': '${categoryName}' }}";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        return defaultHttpRequest;
    }
}