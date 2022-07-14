using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.API.Functions.AddOutcomeTransaction;

public class AddOutcomeTransactionPresenter : IAddOutcomeTransactionOutput
{
    public string TransactionId { get; private set; }
    public decimal AccountBalance { get; private set; }
    public decimal CategoryBalance { get; private set; }
    
    public void Results(AddTransactionResponse response)
    {
        TransactionId = response.TransactionId;
        AccountBalance = response.AccountBalance;
        CategoryBalance = response.CategoryBalance;
    }
}