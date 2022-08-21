using System.Collections.Generic;
using KalanMoney.API.Functions.Commons;
using KalanMoney.Domain.UseCases.AccountDashboard;

namespace KalanMoney.API.Functions.AccountDashboard;

public class AccountDashboardPresenter : IAccountDashboardOutput
{
    public string AccountId { get; private set; }
    
    public string AccountName { get; private set; }
    
    public decimal AccountBalance { get; private set; }
    
    public decimal MonthlyIncomes { get; private set; }
    
    public decimal MonthlyOutcomes { get; private set; }
    
    public TransactionResponse[] AccountTransactions { get; private set; }
    
    public Dictionary<string, decimal> CategoriesBalances { get; private set; }

    public void Results(AccountDashboardResponse response)
    {
        AccountId = response.AccountId;
        AccountName = response.AccountName;
        CategoriesBalances = response.CategoriesBalances;

        MapTransactionsFromResponse(response);

        AccountBalance = response.DashboardBalance.AccountBalance;
        MonthlyIncomes = response.DashboardBalance.IncomeBalance;
        MonthlyOutcomes = response.DashboardBalance.OutcomeBalance;
    }

    private void MapTransactionsFromResponse(AccountDashboardResponse response)
    {
        var totalTransactions = response.AccountTransactions?.Length ?? 0;
        AccountTransactions = new TransactionResponse[totalTransactions];

        for (var i = 0; i < totalTransactions; i++)
        {
            var currentItem = response.AccountTransactions![i];

            AccountTransactions[i] = new TransactionResponse(currentItem.Amount, currentItem.Description.Value,
                currentItem.Category.Value, currentItem.TimeStamp.ToDateTime());
        }
    }
}