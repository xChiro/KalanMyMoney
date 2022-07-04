using KalanMoney.Domain.UseCases.OpenAccount;

namespace KalanMoney.API.Functions.OpenAccountFunctions;

public class OpenAccountPresenter : IOpenAccountOutput
{
    public string AccountId { get; private set; }
    
    public decimal AccountBalance { get; private set; }
    
    public void Results(OpenAccountResponse response)
    {
        AccountId = response.AccountId;
        AccountBalance = response.AccountBalance;
    }
}