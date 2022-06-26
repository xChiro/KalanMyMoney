using KalanMoney.Domain.UseCases.OpenAccount;

namespace KalanMoney.Domain.UseCases.Tests.OpenAccountTests;

public class OpenAccountOutputMock : IOpenAccountOutput
{
    public string AccountId { get; private set; }

    public decimal AccountBalance { get; private set; }

    public void Results(OpenAccountResponse openAccountResponse)
    {
        AccountId = openAccountResponse.AccountId;
        AccountBalance = openAccountResponse.AccountBalance;
    }
}