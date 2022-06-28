using KalanMoney.Domain.UseCases.OpenAccount;

namespace GamerZone.UserManagerService.Api.Presenters;

public class OpenAccountPresenter : IOpenAccountOutput
{
    public string Type { get; set; }
    
    public void Results(OpenAccountResponse openAccountResponse)
    {
        throw new System.NotImplementedException();
    }
}