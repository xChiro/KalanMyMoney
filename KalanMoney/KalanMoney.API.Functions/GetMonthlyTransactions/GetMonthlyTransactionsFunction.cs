using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.GetMonthlyTransactions;

public class GetMonthlyTransactionsFunction : BaseRequestFunction
{
    private readonly IGetMonthlyTransactionsInput _getMonthlyTransactionsInput;

    public GetMonthlyTransactionsFunction(IGetMonthlyTransactionsInput getMonthlyTransactionsInput)
    {
        _getMonthlyTransactionsInput = getMonthlyTransactionsInput;
    }

    [FunctionName("GetMonthlyTransactionsFunction")]
    public Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/transactions")]
        HttpRequest req, ILogger log)
    {
        if (!req.Query.TryGetValue("accountId", out var accountId))
            return Task.FromResult<IActionResult>(new BadRequestObjectResult("AccountId query param is required"));

        if (!req.Query.TryGetValue("year", out var year))
            return Task.FromResult<IActionResult>(new BadRequestObjectResult("Year query param is required"));

        if (!req.Query.TryGetValue("month", out var month))
            return Task.FromResult<IActionResult>(new BadRequestObjectResult("Year query param is required"));

        if (!TryGetOwnerId(req, out var ownerId)) return Task.FromResult<IActionResult>(new UnauthorizedResult());

        try
        {
            var category = req.Query.FirstOrDefault(pair => pair.Key.ToLower() == "category").Value.ToString();
            var transactionsFilters = new TransactionsFilters(Convert.ToInt32(year), 
                Convert.ToInt32(month), string.IsNullOrEmpty(category) ? null : category);
            
            var getMonthlyTransactionsRequest =
                new GetMonthlyTransactionsRequest(accountId, ownerId, transactionsFilters);

            var output = new GetMonthlyTransactionsPresenter();
            _getMonthlyTransactionsInput.Execute(getMonthlyTransactionsRequest, output);

            return Task.FromResult<IActionResult>(new OkObjectResult(output.Transactions));
        }
        catch (AccountNotFoundException)
        {
            return Task.FromResult<IActionResult>(new BadRequestObjectResult("Account not found"));
        }
        catch (Exception ex) when (ex is ArgumentNullException | ex is FormatException | ex is OverflowException)
        {
            log.LogInformation("Bad Request");
            return Task.FromResult<IActionResult>(new BadRequestResult());
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return Task.FromResult<IActionResult>(new InternalServerErrorResult());
        }
    }
}