using KalanMoney.Domain.Entities;
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
        var request = AccountDashboardResponse.Create(account.Id, account.Name, account.Balance, account.Transactions.Items, categories);

        output.Results(request);
    }

    private static Dictionary<string, decimal> MapCategoriesToDictionary(TransactionCollection transactions)
    {
        var categories = new Dictionary<string, decimal>();

        foreach (var transaction in transactions.Items)
        {
            if (categories.ContainsKey(transaction.Category.Value.ToLower()))
            {
                categories[transaction.Category.Value] += transaction.Amount;
            }
            else
            {
                categories.Add(transaction.Category.Value.ToLower(), transaction.Amount);
            }
        }

        return categories;
    }
}