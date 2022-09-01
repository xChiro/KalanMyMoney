using KalanMoney.API.Functions.GetMonthlyTransactions;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.GetMonthlyTransactionsTests;

public class GetMonthlyTransactionsFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_get_monthly_transactions_without_queries_params()
    {
        // Arrange
        var getMonthlyTransactions = CreateGetMonthlyTransactionsMock();

        var sut = new GetMonthlyTransactionsFunction(getMonthlyTransactions.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void Try_to_get_monthly_transactions_with_not_account_id_param()
    {
        // Arrange
        var queriesParams = new Dictionary<string, string>();
        queriesParams.Add("month", "12");
        queriesParams.Add("year", "2022");
        
        var defaultHttpRequest = CreateHttpRequestQueryParams(queriesParams);
        
        var getMonthlyTransactions = CreateGetMonthlyTransactionsMock();
        var sut = new GetMonthlyTransactionsFunction(getMonthlyTransactions.Object);
        
        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async void Try_to_get_monthly_transactions_with_not_year_param()
    {
        // Arrange
        var queriesParams = new Dictionary<string, string>();
        queriesParams.Add("accountId", Guid.NewGuid().ToString());
        queriesParams.Add("month", "12");
        
        var defaultHttpRequest = CreateHttpRequestQueryParams(queriesParams);
        
        var getMonthlyTransactions = CreateGetMonthlyTransactionsMock();
        var sut = new GetMonthlyTransactionsFunction(getMonthlyTransactions.Object);
        
        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async void Try_to_get_monthly_transactions_with_not_month_param()
    {
        // Arrange
        var queriesParams = new Dictionary<string, string>();
        queriesParams.Add("accountId", Guid.NewGuid().ToString());
        queriesParams.Add("year", "2022");
        
        var defaultHttpRequest = CreateHttpRequestQueryParams(queriesParams);
        
        var getMonthlyTransactions = CreateGetMonthlyTransactionsMock();
        var sut = new GetMonthlyTransactionsFunction(getMonthlyTransactions.Object);
        
        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async void Get_monthly_transactions_successfully()
    {
        // Arrange
        var queriesParams = new Dictionary<string, string>();
        queriesParams.Add("accountId", Guid.NewGuid().ToString());
        queriesParams.Add("year", "2022");
        queriesParams.Add("month", "12");
        
        var defaultHttpRequest = CreateHttpRequestQueryParams(queriesParams);
        
        var getMonthlyTransactions = CreateGetMonthlyTransactionsMock();
        var sut = new GetMonthlyTransactionsFunction(getMonthlyTransactions.Object);
        
        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    private static Mock<IGetMonthlyTransactionsInput> CreateGetMonthlyTransactionsMock()
    {
        var getMonthlyTransactions = new Mock<IGetMonthlyTransactionsInput>();
        
        getMonthlyTransactions.Setup(x =>
            x.Execute( It.IsAny<GetMonthlyTransactionsRequest>(), It.IsAny<IGetMonthlyTransactionsOutput>()));

        return getMonthlyTransactions;
    }
}