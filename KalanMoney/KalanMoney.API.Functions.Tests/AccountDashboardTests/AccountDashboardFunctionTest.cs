using System.Diagnostics.CodeAnalysis;
using KalanMoney.API.Functions.AccountDashboard;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace KalanMoney.API.Functions.Test.AccountDashboardTests;

public class AccountDashboardFunctionTest : BaseApiTest
{
    [Fact]
    public async void Try_to_get_an_account_dashboard_without_authorization_token_return_unauthorized()
    {
        // Arrange
        var dashboardInput = new Mock<IAccountDashboardInput>();
        var sut = new AccountDashboardFunction(dashboardInput.Object);
        
        var httpContext = new DefaultHttpContext();
        var httpRequest = new DefaultHttpRequest(httpContext);

        // Act
        var result = await sut.RunAsync(httpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void Try_to_get_an_account_dashboard_with_empty_token_return_unauthorized()
    {
        // Arrange
        var dashboardInput = new Mock<IAccountDashboardInput>();
        var sut = new AccountDashboardFunction(dashboardInput.Object);
        
        var token = StringValues.Empty;
        var httpRequest = CreateHttpRequestNotBody(token);

        // Act
        var result = await sut.RunAsync(httpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async void Try_to_get_an_account_dashboard_with_empty_sub_in_token_return_unauthorized()
    {
        // Arrange
        var dashboardInput = new Mock<IAccountDashboardInput>();
        var sut = new AccountDashboardFunction(dashboardInput.Object);
        
        const string token =
            "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiIsImtpZCI6IjFkYjdhN2FlOTJjZjdmZWI4MmZlY2NkZGMwZDhiM2UyIn0.eyJpc3MiOiJodHRwczovL2lkcC5sb2NhbCIsImF1ZCI6Im15X2NsaWVudF9hcHAiLCJzdWIiOiIiLCJleHAiOjE2NTgxMTEyMDUsImlhdCI6MTY1ODExMDkwNX0.q0FRmR2hsVRFDurHIu4wnkmjSZN0bsjE74RumwsPGn8lwhcuWtLPJrhvMcxVaVeY2YYUBVgbZimNjHnbUiLxSA";
        var httpRequest = CreateHttpRequestNotBody(token);

        // Act
        var result = await sut.RunAsync(httpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void Try_to_get_an_account_dashboard_from_unexciting_owner_id_return_not_found()
    {
        // Arrange
        var dashboardInput = new Mock<IAccountDashboardInput>();
        dashboardInput.Setup(x => x.Execute(It.IsAny<string>(), 
            It.IsAny<IAccountDashboardOutput>())).Throws<AccountNotFoundException>();

        var sut = new AccountDashboardFunction(dashboardInput.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void Get_dashboard_successfully()
    {
        // Arrange
        var dashboardInput = new Mock<IAccountDashboardInput>();
        dashboardInput.Setup(x => x.Execute(It.IsAny<string>(), 
            It.IsAny<IAccountDashboardOutput>()));

        var sut = new AccountDashboardFunction(dashboardInput.Object);
        var defaultHttpRequest = CreateHttpRequestNotBody();

        // Act
        var result = await sut.RunAsync(defaultHttpRequest, new Mock<ILogger>().Object);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}