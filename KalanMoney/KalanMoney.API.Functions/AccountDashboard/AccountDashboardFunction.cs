using System.Threading.Tasks;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KalanMoney.API.Functions.AccountDashboard;

public class AccountDashboardFunction
{
    private readonly IAccountDashboardInput _dashboardInputObject;

    public AccountDashboardFunction(IAccountDashboardInput dashboardInputObject)
    {
        _dashboardInputObject = dashboardInputObject;
    }

    [FunctionName("AccountDashboardFunction")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/dashboard")]
        HttpRequest req, ILogger log)
    {
        var tokenHandler = new TokenHandler(req);
        
        if (!tokenHandler.TryGetSubjectFromToken(out var ownerId)) return new UnauthorizedResult();
        var accountDashboardPresenter = new AccountDashboardPresenter();
        
        try
        {
            _dashboardInputObject.Execute(ownerId, accountDashboardPresenter);
        }
        catch (AccountNotFoundException)
        {
            return new NotFoundResult();
        } 
        
        return new OkObjectResult(accountDashboardPresenter);
    }
}