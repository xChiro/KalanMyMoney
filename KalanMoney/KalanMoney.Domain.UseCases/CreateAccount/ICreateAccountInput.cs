using KalanMoney.Domain.UseCases.CreateAccount;

namespace KalanMoney.Domain.UseCases.Adapters;

public interface ICreateAccountInput
{
    public void Execute(CreateAccountRequest requestModel, ICreateAccountOutput createAccountOutput);
}