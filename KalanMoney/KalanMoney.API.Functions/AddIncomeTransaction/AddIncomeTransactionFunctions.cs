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

public class AddIncomeTransactionFunctions : BaseFunctions<AddIncomeTransactionFunctionRequest>
{
    private readonly IAddIncomeTransactionInput _addIncomeTransactionInput;

    public AddIncomeTransactionFunctions(IAddIncomeTransactionInput addIncomeTransactionInput)
    {
        _addIncomeTransactionInput = addIncomeTransactionInput;
    }

    [FunctionName("AddIncomeTransactionFunctions")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        try
        {
            var addIncomeTransactionFunctionRequest =  await DeserializeRequest(req);
            var addTransactionRequest = new AddTransactionRequest(addIncomeTransactionFunctionRequest.AccountId, 
                addIncomeTransactionFunctionRequest.CategoryId, addIncomeTransactionFunctionRequest.Amount);
            
            var outputPresenter = new AddIncomeTransactionPresenter();
            _addIncomeTransactionInput.Execute(addTransactionRequest, outputPresenter);
            
            return new OkObjectResult(outputPresenter);
        }
        catch (Exception e) when(e is AccountNotFoundException | e is CategoryNotFoundException)
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