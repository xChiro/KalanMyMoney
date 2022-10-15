using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.GetAccountDashboard;

public record AccountDashboardResponse
{
    public string AccountId { get; }
    public string AccountName { get; }
    public Transaction[]? AccountTransactions { get; }
    public CategoriesBalances CategoriesesBalances { get; }
    public DashboardBalance DashboardBalance { get; }

    private AccountDashboardResponse(string accountId, string accountName, Transaction[]? accountTransactions,
        CategoriesBalances categoriesesBalances, DashboardBalance dashboardBalance)
    {
        AccountId = accountId;
        AccountName = accountName;
        AccountTransactions = accountTransactions;
        CategoriesesBalances = categoriesesBalances;
        DashboardBalance = dashboardBalance;
    }

    public static AccountDashboardResponse Create(string accountId, AccountName accountName, Balance accountBalance, 
        Transaction[]? accountTransactions, CategoriesBalances categoriesBalances)
    {
        var incomeBalance = 0m;
        var outcomeBalance = 0m;

        if (accountTransactions != null)
        {
            incomeBalance = accountTransactions.Where(x => x.Amount >= 0).Sum(x => x.Amount);
            outcomeBalance = accountTransactions.Where(x => x.Amount < 0).Sum(x => x.Amount);
        }

        var dashboardBalance = new DashboardBalance(accountBalance.Amount, incomeBalance, outcomeBalance);

        return new AccountDashboardResponse(accountId, accountName.Value, accountTransactions, categoriesBalances,
            dashboardBalance);
    }
}