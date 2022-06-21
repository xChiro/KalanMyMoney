using KalanMoney.Domain.UseCases.Adapters;

namespace KalanMoney.Domain.UseCases.Test.Adapters;

public class CreateAccountOutputMock : ICreateAccountOutput
{
    public string AccountId { get; private set; }

    public decimal AccountBalance { get; private set; }
    
    public void Results(string accountId, decimal accountBalance)
    {
        AccountId = accountId;
        AccountBalance = accountBalance;
    }
}