using KalanMoney.API.Functions.GetCategoriesByAccount;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.GetCategoriesByAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.GetCategoriesByAccountTests;

public class GetCategoriesFunctionTest : BaseApiTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async void Try_to_get_categories_with_invalid_accountId(string accountId)
    {
        // Arrange
        var getMonthlyTransactions = new Mock<IGetCategoriesByAccountInput>();

        var sut = new GetCategoriesFunction(getMonthlyTransactions.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, accountId, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async void Try_to_get_categories_from_unexciting_account()
    {
        // Arrange 
        var getMonthlyTransactions = new Mock<IGetCategoriesByAccountInput>();
        getMonthlyTransactions.Setup(x =>
            x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IGetCategoriesByAccountOutput>()))
            .Throws(() => new AccountNotFoundException());
        
        
        var sut = new GetCategoriesFunction(getMonthlyTransactions.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, "0", new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async void Get_categories_from_unexciting_account_successfully()
    {
        // Arrange 
        var getMonthlyTransactions = new Mock<IGetCategoriesByAccountInput>();
        
        var sut = new GetCategoriesFunction(getMonthlyTransactions.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, "0", new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}