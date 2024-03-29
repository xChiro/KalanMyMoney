using System;
using System.Threading.Tasks;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.AddIncomeTransaction;

public class AddIncomeTransactionRequestFunction : BaseRequestFunction
{
    private readonly IAddIncomeTransactionInput _addIncomeTransactionInput;

    public AddIncomeTransactionRequestFunction(IAddIncomeTransactionInput addIncomeTransactionInput)
    {
        _addIncomeTransactionInput = addIncomeTransactionInput;
    }

    [FunctionName("AddIncomeTransactionRequestFunction")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts/transactions/income")] HttpRequest req, ILogger log)
    {
        if (!TryGetOwnerId(req, out var ownerId)) return new UnauthorizedResult();

        try
        {
            var request =  await DeserializeRequest<AddIncomeTransactionFunctionRequest>(req);
            var addTransactionRequest = new AddTransactionRequest(request.AccountId, ownerId, request.Amount, request.TransactionDescription, request.Category);
            
            var outputPresenter = new AddIncomeTransactionPresenter();
            _addIncomeTransactionInput.Execute(addTransactionRequest, outputPresenter);
            
            return new OkObjectResult(outputPresenter);
        }
        catch (Exception e) when(e is AccountNotFoundException | e is CategoryNotFoundException)
        {
            return new NotFoundResult();
        }
        catch (Exception ex) when (ex is JsonException | ex is InvalidCastException | ex is ArgumentNullException)
        {   
            log.LogInformation("Bad Request");
            return new BadRequestResult();
        }
    }
}