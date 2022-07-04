using System.Text;
using System.Web.Http;
using Castle.Components.DictionaryAdapter.Xml;
using KalanMoney.API.Functions.AccountFunctions;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.OpenAccountTests;

public class OpenAccountFunctionTest
{
    [Fact]
    public async void Try_to_open_an_account_with_invalid_json_request_return_bad_request()
    {
        // Arrange
        var accountInput = CreateOpenAccountUseCase();

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "{ 'badRequest': 'Test' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<BadRequestErrorMessageResult>(result);
    }

    [Fact]
    public async void Try_to_open_an_account_with_an_invalid_name_return_bad_request()
    {
        // Arrange
        var accountInput = CreateOpenAccountUseCase();

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "{ 'AccountName': 'The name of this accounts its to long to be permitted.' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
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
        var accountInput = CreateOpenAccountUseCase();

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "'AccountName': 'The name of this accounts its to long to be permitted.' }";
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
        var accountInput = CreateOpenAccountUseCase();

        var sut = new OpenAccountFunction(accountInput);
        
        const string requestBody = "{ 'AccountName': 'Test Account.' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        var loggerMock = new Mock<ILogger>(); 

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, loggerMock.Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        
    }

    private static OpenAccountUseCase CreateOpenAccountUseCase()
    {
        var accountRepository = new Mock<IAccountCommandsRepository>();
        var accountInput = new OpenAccountUseCase(accountRepository.Object);
        
        return accountInput;
    }

    private static DefaultHttpRequest CreateHttpRequest(string requestBody)
    {
        var badRequestBytes = Encoding.ASCII.GetBytes(requestBody);
        var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(badRequestBytes)
        };
        
        return defaultHttpRequest;
    }
}