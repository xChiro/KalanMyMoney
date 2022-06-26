using KalanMoney.Domain.UseCases.AddIncomeTransaction;

namespace KalanMoney.Domain.UseCases.Tests.AddIncomeTransactionTests;

public class AddIncomeTransactionOutputMock : IAddIncomeTransactionOutput
{
    public string TransactionId { get; private set; }
    
    public decimal AccountBalance { get; private set; }
    
    public decimal CategoryBalance { get; private set; }
    
    public void Results(AddIncomeTransactionResponse response)
    {
        TransactionId = response.TransactionId;
        CategoryBalance = response.CategoryBalance;
        AccountBalance = response.AccountBalance;
    }
}