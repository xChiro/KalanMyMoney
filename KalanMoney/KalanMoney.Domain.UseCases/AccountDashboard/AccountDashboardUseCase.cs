using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public class AccountDashboardUseCase : IAccountDashboardInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public AccountDashboardUseCase(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    /// <exception cref="AccountNotFoundException"></exception>
    public void Execute(string accountId, IAccountDashboardOutput output)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var account = _accountQueriesRepository.GetTransactions(accountId,today, today.AddDays(-30));

        if (account == null) throw new AccountNotFoundException();
    }
}