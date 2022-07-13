using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.CreateCategory;

public class CreateCategoryUseCase : ICreateCategoryInput
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
        var account = _accountQueriesRepository.GetAccountOnly(request.AccountId);
        
        if (account == null) throw new AccountNotFoundException();

        var category = new FinancialCategory(categoryName, request.AccountId, account.Owner);
        _categoryCommandsRepository.CreateCategory(category);
        
        output.Response(category.Id);
    }
}