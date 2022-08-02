using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;

namespace KalanMoney.Domain.UseCases.Tests.AddIncomeTransactionTests;

public class AddIncomeTransactionOutputMock : IAddIncomeTransactionOutput
{
    public string TransactionId { get; private set; }
    
    public decimal AccountBalance { get; private set; }
    
    public void Results(AddTransactionResponse response)
    {
        TransactionId = response.TransactionId;
        AccountBalance = response.AccountBalance;
    }
}