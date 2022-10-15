using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.GetAccountDashboard;

public class GetAccountDashboard : IAccountDashboardInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public GetAccountDashboard(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    /// <exception cref="AccountNotFoundException"></exception>
    public void Execute(string ownerId, IAccountDashboardOutput output)
    {
        var transactionsFilters = DateRangeFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccountByOwner(ownerId, transactionsFilters);

        if (account == null) throw new AccountNotFoundException();

        var categories = CategoriesBalances.CreateFromTransactions(account.Transactions);
        var request = AccountDashboardResponse.Create(account.Id, account.Name, account.Balance, account.Transactions.Items, categories);

        output.Results(request);
    }
}