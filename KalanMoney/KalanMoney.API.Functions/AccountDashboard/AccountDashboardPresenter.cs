using System.Collections.Generic;
using KalanMoney.API.Functions.Commons;
using KalanMoney.Domain.UseCases.AccountDashboard;

namespace KalanMoney.API.Functions.AccountDashboard;

public class AccountDashboardPresenter : IAccountDashboardOutput
{
    public string AccountId { get; private set; }
    
    public string AccountName { get; private set; }

    public TransactionResponse[] AccountTransactions { get; set; }
    
    public Dictionary<string, decimal> CategoriesBalances { get; set; }

    public void Results(AccountDashboardResponse response)
    {
        AccountId = response.AccountId;
        AccountName = response.AccountName;

        var totalTransactions = response.AccountTransactions?.Length ?? 0;
        AccountTransactions = new TransactionResponse[totalTransactions];

        for (var i = 0; i < totalTransactions; i++)
        {
            var currentItem = response.AccountTransactions![i];

            AccountTransactions[i] = new TransactionResponse(currentItem.Amount, currentItem.Description.Value,
                currentItem.Category.Value, currentItem.TimeStamp.ToDateTime());
        }
        
        CategoriesBalances = response.CategoriesBalances;
    }
}