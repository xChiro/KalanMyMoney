using System.Collections;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.AccountDashboard;

namespace KalanMoney.Domain.UseCases.Tests.AccountDashboardTests;

public class AccountDashboardOutputMock : IAccountDashboardOutput
{
    public AccountDashboardRequest AccountDashboardRequest { get; private set; }
    
    public void Results(AccountDashboardRequest request)
    {
        AccountDashboardRequest = request;
    }
}