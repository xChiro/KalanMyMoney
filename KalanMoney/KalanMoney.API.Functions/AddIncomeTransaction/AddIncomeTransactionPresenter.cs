using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.API.Functions.AddIncomeTransaction;

public record AddIncomeTransactionPresenter : IAddIncomeTransactionOutput
{
    public void Results(AddTransactionResponse response)
    {
        AccountBalance = response.AccountBalance;
        CategoryBalance = response.CategoryBalance;
        TransactionId = response.TransactionId;
    }

    public string TransactionId { get; private set; }

    public decimal CategoryBalance { get; private set; }

    public decimal AccountBalance { get; private set; }
}