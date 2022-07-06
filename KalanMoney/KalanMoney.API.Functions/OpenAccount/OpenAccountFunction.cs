using System;
using System.Threading.Tasks;
using System.Web.Http;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.OpenAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions.OpenAccount;

public class OpenAccountFunction : BaseFunctions<OpenAccountFunctionRequest>
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
            var data = await DeserializeRequest(req);
            
            var presenter = new OpenAccountPresenter();
            var createAccountRequest = new CreateAccountRequest(Guid.NewGuid().ToString(), "A name here", data.AccountName);

            _openAccountInput.Execute(createAccountRequest, presenter);

            return new OkObjectResult(new
            {
                presenter.AccountId,
                presenter.AccountBalance,
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
}