using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.AddOutcomeTransaction;

public class AddOutcomeTransactionUseCase : IAddOutcomeTransactionOutput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly ICategoryQueriesRepository _categoryQueriesRepository;

    public AddOutcomeTransactionUseCase(IAccountQueriesRepository accountQueriesRepository,
        ICategoryQueriesRepository categoryQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _categoryQueriesRepository = categoryQueriesRepository;
    }

    public void Execute(AddTransactionRequest addTransactionRequest)
    {
        var account = _accountQueriesRepository.GetAccountById(addTransactionRequest.AccountId);
        var category = _categoryQueriesRepository.GetCategoryById(addTransactionRequest.CategoryId);

        if (account == null) throw new AccountNotFoundException();
        if (category == null) throw new CategoryNotFoundException();
    }
}