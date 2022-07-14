using System;
using System.IO;
using System.Threading.Tasks;
using KalanMoney.API.Functions.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.AddOutcomeTransaction;

public class AddOutcomeTransactionFunctions : BaseFunctions<AddOutcomeTransactionFunctionRequest>
{
    private readonly IAddOutcomeTransactionInput _addOutcomeTransactionInput;

    public AddOutcomeTransactionFunctions(IAddOutcomeTransactionInput addOutcomeTransactionInput)
    {
        _addOutcomeTransactionInput = addOutcomeTransactionInput;
    }

    [FunctionName("AddOutcomeTransactionFunctions")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        try
        {
            var request = await DeserializeRequest(req);
            var output = new AddOutcomeTransactionPresenter();

            _addOutcomeTransactionInput.Execute(
                new AddTransactionRequest(request.AccountId, request.CategoryId, request.Amount), output);

            return new OkObjectResult(new
            {
                output.TransactionId,
                output.AccountBalance,
                output.CategoryBalance,
            });
        }
        catch (Exception ex) when (ex is AccountNotFoundException | ex is CategoryNotFoundException)
        {
            return new NotFoundResult();
        }
        catch (Exception ex) when (ex is JsonException | ex is InvalidCastException)
        {
            log.LogInformation("Bad Request");
            return new BadRequestResult();
        }
    }
}