using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.CreateCategory;

public class CreateCategoryUseCase
{
    private readonly ICategoryCommandsRepository _categoryCommandsRepository;
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public CreateCategoryUseCase(ICategoryCommandsRepository categoryCommandsRepository,
        IAccountQueriesRepository accountQueriesRepository)
    {
        _categoryCommandsRepository = categoryCommandsRepository;
        _accountQueriesRepository = accountQueriesRepository;
    }
    
    public void Execute(CreateCategoryRequest request, ICreateCategoryOutput output)
    {
        var categoryName = AccountName.Create(request.CategoryName);
        
        if (!_accountQueriesRepository.AccountExists(request.AccountId)) throw new AccountNotFoundException();
        
        
        output.Response(Guid.NewGuid().ToString());
    }
}