using KalanMoney.Domain.UseCases.GetAccountDashboard;

namespace KalanMoney.Domain.UseCases.Tests.AccountDashboardTests;

public class AccountDashboardOutputMock : IAccountDashboardOutput
{
    public AccountDashboardResponse? AccountDashboardResponse { get; private set; }
    
    public void Results(AccountDashboardResponse response)
    {
        AccountDashboardResponse = response;
    }
}