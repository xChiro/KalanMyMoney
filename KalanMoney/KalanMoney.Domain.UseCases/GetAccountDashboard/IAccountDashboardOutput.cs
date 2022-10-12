namespace KalanMoney.Domain.UseCases.GetAccountDashboard;

public interface IAccountDashboardOutput
{
    void Results(AccountDashboardResponse response);
}