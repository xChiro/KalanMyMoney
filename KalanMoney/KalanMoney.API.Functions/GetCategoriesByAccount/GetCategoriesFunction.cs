using System;
using System.IO;
using System.Threading.Tasks;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.GetCategoriesByAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.GetCategoriesByAccount;

public class GetCategoriesFunction : BaseRequestFunction
{
    private readonly IGetCategoriesByAccountInput _getCategoriesByAccountInput;

    public GetCategoriesFunction(IGetCategoriesByAccountInput categoriesByAccountInput)
    {
        _getCategoriesByAccountInput = categoriesByAccountInput;
    }

    [FunctionName("GetCategoriesFunction")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/{id}/categories")]
        HttpRequest req,
        string id, ILogger log)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new BadRequestResult();

        if (!TryGetOwnerId(req, out var ownerId)) return new UnauthorizedResult();

        try
        {
            var presenter = new GetCategoriesPresenter();
            _getCategoriesByAccountInput.Execute(id, ownerId, presenter);
            return new OkObjectResult(presenter);
        }
        catch (AccountNotFoundException ex)
        {
            return new NotFoundResult();
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message, ex);
            return new BadRequestResult();
        }
    }
}