using KalanMoney.API.Functions.AccountDashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.AccountDashboardTests;

public class AccountDashboardFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_get_an_account_dashboard_from_unexciting_owner_id_return_not_found()
    {
        // Arrange
        var sut = new AccountDashboardFunctions();
        var defaultHttpRequest = CreateHttpRequest(string.Empty);

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory(Skip = "Under construction! by xChiro")]
    [InlineData("{}")]
    public async void Try_to_get_an_account_dashboard_with_wrong_json_body_return_bad_request(string body)
    {
        // Arrange
        var sut = new AccountDashboardFunctions();
        var defaultHttpRequest = CreateHttpRequest(body);

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}