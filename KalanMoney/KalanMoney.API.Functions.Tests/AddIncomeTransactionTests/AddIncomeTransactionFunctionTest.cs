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

public class AddIncomeTransactionFunctionTest
{
    [Fact]
    public async void Try_to_add_an_income_transaction_in_an_unexciting_account_return_not_found_request()
    {
        // Arrange
        var addIncomeTransactionInput = SetupAddIncomeTransactionInputMock(new AccountNotFoundException());

        var sut = new AddIncomeTransactionFunctions(addIncomeTransactionInput.Object);
        var defaultHttpRequest = CreateDefaultHttpRequestNoBody();

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

        var sut = new AddIncomeTransactionFunctions(addIncomeTransactionInput.Object);
        var defaultHttpRequest = CreateDefaultHttpRequestNoBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private static DefaultHttpRequest CreateDefaultHttpRequestNoBody()
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