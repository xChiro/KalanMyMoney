using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public class AccountDashboardUseCase : IAccountDashboardInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public AccountDashboardUseCase(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    /// <exception cref="AccountNotFoundException"></exception>
    public void Execute(string ownerId, IAccountDashboardOutput output)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccountByOwner(ownerId, transactionsFilters);

        if (account == null) throw new AccountNotFoundException();

        var categories = MapCategoriesToDictionary(account.Transactions);
        var request = new AccountDashboardResponse(account.Id, account.Name.Value, account.Transactions.Items, categories);
        
        output.Results(request);
    }

    private static Dictionary<Category, Balance> MapCategoriesToDictionary(TransactionCollection transactions)
    {
        return transactions.Items.ToDictionary(current => current.Category, current => new Balance(current.Amount));
    }

}