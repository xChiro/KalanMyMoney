using System.Collections;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.AccountDashboard;

namespace KalanMoney.Domain.UseCases.Tests.AccountDashboardTests;

public class AccountDashboardOutputMock : IAccountDashboardOutput
{
    public AccountDashboardResponse AccountDashboardResponse { get; private set; }
    
    public void Results(AccountDashboardResponse response)
    {
        AccountDashboardResponse = response;
    }
}