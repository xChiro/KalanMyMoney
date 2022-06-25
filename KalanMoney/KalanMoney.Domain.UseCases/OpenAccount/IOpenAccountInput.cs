using KalanMoney.Domain.UseCases.OpenAccount;

namespace KalanMoney.Domain.UseCases.Adapters;

public interface IOpenAccountInput
{
    public void Execute(CreateAccountRequest requestModel, IOpenAccountOutput openAccountOutput);
}