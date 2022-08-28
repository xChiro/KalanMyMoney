using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.Domain.UseCases.Tests.AddOutcomeTransactionTests;

public class AddOutcomeTransactionOutput : IAddOutcomeTransactionOutput
{
    public string? TransactionId { get; private set; }
    public decimal AccountBalance { get; private set; }
    
    public void Results(AddTransactionResponse response)
    {
        TransactionId = response.TransactionId;
        AccountBalance = response.AccountBalance;
    }
}