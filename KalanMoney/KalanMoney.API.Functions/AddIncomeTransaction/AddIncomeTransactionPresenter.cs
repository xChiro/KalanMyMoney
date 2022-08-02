using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.API.Functions.AddIncomeTransaction;

public record AddIncomeTransactionPresenter : IAddIncomeTransactionOutput
{
    public void Results(AddTransactionResponse response)
    {
        AccountBalance = response.AccountBalance;
        TransactionId = response.TransactionId;
    }

    public string TransactionId { get; private set; }

    public decimal AccountBalance { get; private set; }
}