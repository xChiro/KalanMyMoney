using System.Text;
using KalanMoney.API.Functions.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.AddOutcomeTransactionTests;

public class AddIncomeTransactionFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_add_an_outcome_transaction_from_unexciting_account_return_not_found()
    {
        // Arrange
        var addOutcomeTransactionInput = SetupThrowableAddOutcomeTransactionInput<AccountNotFoundException>();

        var sut = new AddOutcomeTransactionRequestFunction(addOutcomeTransactionInput.Object);
        const string requestBody = "{ 'AccountId': '' }";
        var defaultHttpRequest = CreateHttpRequest(requestBody);

        // Act
        var response = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async void Try_to_add_an_outcome_transaction_wrong_request_format_return_bad_request()
    {
        // Arrange
        var addOutcomeTransactionInput = new Mock<IAddOutcomeTransactionInput>();
        var sut = new AddOutcomeTransactionRequestFunction(addOutcomeTransactionInput.Object);
        const string requestBody = "{ 'AccountId': '' ";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        // Act
        var response = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);
        
        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Theory]
    [InlineData("{'badRequest': ")]
    [InlineData("{'badRequest': }")]
    [InlineData("{'AccountId': 1, 'CategoryId': 1, 'Amount': null}")]
    public async void Try_to_add_a_new_outcome_transaction_with_a_wrong_json_body_returns_bad_request(string jsonBody)
    {
        // Arrange
        var addOutcomeTransactionInput = new Mock<IAddOutcomeTransactionInput>();
        addOutcomeTransactionInput.Setup(x => x.Execute(It.IsAny<AddTransactionRequest>(),
            It.IsAny<IAddOutcomeTransactionOutput>()));

        var sut = new AddOutcomeTransactionRequestFunction(addOutcomeTransactionInput.Object);
        var defaultHttpContext = CreateHttpRequest(jsonBody);

        // Act
        var result = await sut.RunAsync(defaultHttpContext, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async void Add_a_new_outcome_transaction_successfully()
    {
        // Arrange
        var addOutcomeTransactionInput = new Mock<IAddOutcomeTransactionInput>();
        var sut = new AddOutcomeTransactionRequestFunction(addOutcomeTransactionInput.Object);
        const string requestBody = "{'AccountId': 1, 'CategoryId': 1, 'Amount': 100}";
        var defaultHttpRequest = CreateHttpRequest(requestBody);
        
        // Act
        var response = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(response);
    }

    private static Mock<IAddOutcomeTransactionInput> SetupThrowableAddOutcomeTransactionInput<T>() where T : Exception, new()
    {
        var addOutcomeTransactionInput = new Mock<IAddOutcomeTransactionInput>();
        addOutcomeTransactionInput.Setup(x =>
                x.Execute(It.IsAny<AddTransactionRequest>(), It.IsAny<IAddOutcomeTransactionOutput>()))
            .Throws<T>();
        
        return addOutcomeTransactionInput;
    }
}