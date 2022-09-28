using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.GetCategoriesByAccount;

public class GetCategoriesByAccountUseCase : IGetCategoriesByAccountInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public GetCategoriesByAccountUseCase(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    public void Execute(string accountId, string ownerId, IGetCategoriesByAccountOutput output)
    {
        if (string.IsNullOrEmpty(accountId) || string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException("Invalid account id value", nameof(accountId));

        if (string.IsNullOrEmpty(ownerId) || string.IsNullOrWhiteSpace(ownerId))
            throw new ArgumentException("Invalid owner id value", nameof(accountId));
        
        var categories = _accountQueriesRepository.GetCategoriesByAccount(accountId, ownerId);

        output.Results(categories);
    }
}