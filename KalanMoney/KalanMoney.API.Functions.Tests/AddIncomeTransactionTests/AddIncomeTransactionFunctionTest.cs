using KalanMoney.API.Functions.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.AddIncomeTransactionTests;

public class AddIncomeTransactionFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_add_an_income_transaction_in_an_unexciting_account_return_not_found_request()
    {
        // Arrange
        var addIncomeTransactionInput = SetupAddIncomeTransactionInputMock(new AccountNotFoundException());

        var sut = new AddIncomeTransactionRequestFunction(addIncomeTransactionInput.Object);
        var defaultHttpRequest = BaseApiTest.CreateHttpRequest("{'AccountId': 1, 'CategoryId': 1, 'Amount': '110'}");

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void Try_to_add_an_income_transaction_in_an_unexciting_category_return_not_found_request()
    {
        // Arrange
        var addIncomeTransactionInput = SetupAddIncomeTransactionInputMock(new CategoryNotFoundException());

        var sut = new AddIncomeTransactionRequestFunction(addIncomeTransactionInput.Object);
        var defaultHttpRequest = BaseApiTest.CreateHttpRequest("{'AccountId': 1, 'CategoryId': 1, 'Amount': '110'}");

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("{'badRequest': ")]
    [InlineData("{'badRequest': }")]
    [InlineData("{'AccountId': 1, 'CategoryId': 1, 'Amount': null}")]
    public async void Try_to_add_a_new_income_transaction_with_wrong_json_body_returns_bad_request(string jsonBody)
    {
        // Arrange
        var addIncomeTransactionInput = new Mock<IAddIncomeTransactionInput>();
        addIncomeTransactionInput.Setup(x => x.Execute(It.IsAny<AddTransactionRequest>(),
            It.IsAny<IAddIncomeTransactionOutput>()));

        var sut = new AddIncomeTransactionRequestFunction(addIncomeTransactionInput.Object);
        var defaultHttpContext = CreateHttpRequest(jsonBody);

        // Act
        var result = await sut.RunAsync(defaultHttpContext, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async void Add_a_new_income_transaction_successfully()
    {
        // Arrange
        var addIncomeTransactionInput = new Mock<IAddIncomeTransactionInput>();
        var sut = new AddIncomeTransactionRequestFunction(addIncomeTransactionInput.Object);
        var httpRequest = CreateHttpRequest("{'AccountId': 1, 'CategoryId': 1, 'Amount': 100}");
        
        // Act
        var result = await sut.RunAsync(httpRequest, new Mock<ILogger>().Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    private static DefaultHttpRequest CreateHttpRequestWithoutNoBody()
    {
        var defaultHttpContext = new DefaultHttpContext();
        var defaultHttpRequest = new DefaultHttpRequest(defaultHttpContext);
        
        return defaultHttpRequest;
    }

    private static Mock<IAddIncomeTransactionInput> SetupAddIncomeTransactionInputMock(Exception exception)
    {
        var addIncomeTransactionInput = new Mock<IAddIncomeTransactionInput>();
        addIncomeTransactionInput
            .Setup(x => x.Execute(It.IsAny<AddTransactionRequest>(), It.IsAny<IAddIncomeTransactionOutput>()))
            .Throws(exception);
        
        return addIncomeTransactionInput;
    }
}