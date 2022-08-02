using System.Collections.Generic;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.AccountDashboard;

namespace KalanMoney.API.Functions.AccountDashboard;

public class AccountDashboardPresenter : IAccountDashboardOutput
{
    public string AccountId { get; private set; }
    
    public string AccountName { get; private set; }

    public Transaction[] AccountTransactions { get; set; }
    
    public Dictionary<string, decimal> CategoriesBalances { get; set; }

    public void Results(AccountDashboardResponse response)
    {
        AccountId = response.AccountId;
        AccountName = response.AccountName;
        AccountTransactions = response.AccountTransactions;
        CategoriesBalances = response.CategoriesBalances;
    }
}