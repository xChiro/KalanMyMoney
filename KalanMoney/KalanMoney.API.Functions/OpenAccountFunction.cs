using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using KalanMoney.API.Functions.Models;
using KalanMoney.API.Functions.Presenters;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.OpenAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions;

public class OpenAccountFunction
{
    private readonly IOpenAccountInput _openAccountInput;

    public OpenAccountFunction(IOpenAccountInput input)
    {
        _openAccountInput = input;
    }
    
    [FunctionName("OpenAccountFunction")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("Init request to OpenAccountFunction");

        try
        {
            var data = await GetOpenAccountFunctionRequest(req);
            
            var output = new OpenAccountPresenter();
            var createAccountRequest = new CreateAccountRequest(Guid.NewGuid().ToString(), "A name here", data.AccountName);

            _openAccountInput.Execute(createAccountRequest, output);

            return new OkObjectResult(new
            {
                output.AccountId,
                output.AccountBalance,
            });
        }
        catch (Exception ex) when (ex is JsonSerializationException | ex is InvalidCastException)
        {   
            log.LogInformation("Bad Request");
            return new BadRequestResult();
        }
        catch (AccountNameException)
        {
            return new BadRequestErrorMessageResult("Account name contains an invalid format.");
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return new InternalServerErrorResult();
        }
    }

    private static async Task<OpenAccountFunctionRequest> GetOpenAccountFunctionRequest(HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<OpenAccountFunctionRequest>(requestBody);
        
        return data;
    }
}