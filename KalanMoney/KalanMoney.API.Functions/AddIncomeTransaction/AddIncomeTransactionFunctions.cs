using System;
using System.IO;
using System.Threading.Tasks;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.AddIncomeTransaction;

public class AddIncomeTransactionFunctions
{
    private readonly IAddIncomeTransactionInput _addIncomeTransactionInput;

    public AddIncomeTransactionFunctions(IAddIncomeTransactionInput addIncomeTransactionInput)
    {
        _addIncomeTransactionInput = addIncomeTransactionInput;
    }

    [FunctionName("AddIncomeTransactionFunctions")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        try
        {
            var addTransactionRequest = new AddTransactionRequest("AccountId", "CategoryId", decimal.Zero);
            _addIncomeTransactionInput.Execute(addTransactionRequest, new AddIncomeTransactionPresenter());
            
            return new OkResult();
        }
        catch (Exception e) when(e is AccountNotFoundException | e is CategoryNotFoundException)
        {
            return new NotFoundResult();
        }
    }
}