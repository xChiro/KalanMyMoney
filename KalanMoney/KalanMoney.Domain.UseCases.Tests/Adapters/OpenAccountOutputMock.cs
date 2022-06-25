using KalanMoney.Domain.UseCases.OpenAccount;

namespace KalanMoney.Domain.UseCases.Tests.Adapters;

public class OpenAccountOutputMock : IOpenAccountOutput
{
    public string AccountId { get; private set; }

    public decimal AccountBalance { get; private set; }
    
    public void Results(string accountId, decimal accountBalance)
    {
        AccountId = accountId;
        AccountBalance = accountBalance;
    }
}