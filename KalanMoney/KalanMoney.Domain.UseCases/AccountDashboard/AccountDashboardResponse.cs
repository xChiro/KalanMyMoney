using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public record AccountDashboardResponse
{
    public string AccountId { get; }
    public string AccountName { get; }
    public Transaction[]? AccountTransactions { get; }
    public Dictionary<string, decimal> CategoriesBalances { get; }
    public DashboardBalance DashboardBalance { get; }

    private AccountDashboardResponse(string accountId, string accountName, Transaction[]? accountTransactions,
        Dictionary<string, decimal> categoriesBalances, DashboardBalance dashboardBalance)
    {
        AccountId = accountId;
        AccountName = accountName;
        AccountTransactions = accountTransactions;
        CategoriesBalances = categoriesBalances;
        DashboardBalance = dashboardBalance;
    }

    public static AccountDashboardResponse Create(string accountId, AccountName accountName, Balance accountBalance, 
        Transaction[]? accountTransactions, Dictionary<string, decimal> categoriesBalances)
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