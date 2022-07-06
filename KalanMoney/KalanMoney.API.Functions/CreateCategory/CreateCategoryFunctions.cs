using System;
using System.Threading.Tasks;
using System.Web.Http;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.CreateCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KalanMoney.API.Functions.CreateCategory;

public class CreateCategoryFunctions : BaseFunctions<CreateCategoryFunctionRequest>
{
    private readonly ICreateCategoryInput _categoryInput;

    public CreateCategoryFunctions(ICreateCategoryInput categoryInput)
    {
        _categoryInput = categoryInput;
    }

    [FunctionName("CreateCategoryFunctions")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        log.LogInformation("Init request to CreateCategoryFunctions");

        try
        {
            var presenter = new CreateCategoryPresenter();
            var data = await DeserializeRequest(req);
            var categoryRequest = new CreateCategoryRequest(data.AccountId, data.CategoryName);

            _categoryInput.Execute(categoryRequest, presenter);

            return new OkObjectResult(new
            {
                presenter.CategoryId,
            });
        }
        catch (AccountNotFoundException)
        {
            return new NotFoundResult();
        }
        catch (AccountNameException)
        {
            return new BadRequestResult();
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return new InternalServerErrorResult();
        }
    }
}