using KalanMoney.Domain.UseCases.OpenAccount;

namespace KalanMoney.Domain.UseCases.Tests.OpenAccountTests;

public class OpenAccountOutputMock : IOpenAccountOutput
{
    public string AccountId { get; private set; }

    public decimal AccountBalance { get; private set; }

    public void Results(OpenAccountResponse response)
    {
        AccountId = response.AccountId;
        AccountBalance = response.AccountBalance;
    }
}