using System;
using System.Threading.Tasks;
using System.Web.Http;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.GetAccountDashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KalanMoney.API.Functions.AccountDashboard;

public class AccountDashboardFunction : BaseRequestFunction
{
    private readonly IAccountDashboardInput _dashboardInput;

    public AccountDashboardFunction(IAccountDashboardInput dashboardInput)
    {
        _dashboardInput = dashboardInput;
    }

    [FunctionName("AccountDashboardFunction")]
    public Task<IActionResult> RunAsync(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/dashboard")]
        HttpRequest req, ILogger log)
    {
        if (!TryGetOwnerId(req, out var ownerId)) return Task.FromResult<IActionResult>(new UnauthorizedResult());
        
        var accountDashboardPresenter = new AccountDashboardPresenter();
        
        try
        {
            _dashboardInput.Execute(ownerId, accountDashboardPresenter);
        }
        catch (AccountNotFoundException)
        {
            return Task.FromResult<IActionResult>(new NotFoundResult());
        } 
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return Task.FromResult<IActionResult>(new InternalServerErrorResult());
        } 
        
        return Task.FromResult<IActionResult>(new OkObjectResult(accountDashboardPresenter));
    }
}