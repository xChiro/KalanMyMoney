using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public interface IAccountDashboardOutput
{
    void Results(AccountDashboardRequest request);
}