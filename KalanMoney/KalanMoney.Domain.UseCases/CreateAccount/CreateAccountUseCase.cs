using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.Properties;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.CreateAccount;

public class CreateAccountUseCase : ICreateAccountInput
{
    private readonly IAccountCommandsRepository _accountCommands;
    
    public CreateAccountUseCase(IAccountCommandsRepository accountCommands)
    {
        _accountCommands = accountCommands;
    }

    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 155.
    /// </exception>
    public void Execute(CreateAccountRequest requestModel, ICreateAccountOutput createAccountOutput)
    {
        var accountName = AccountName.Create(requestModel.AccountName);
        var financialAccount = new FinancialAccount(accountName, requestModel.OwnerId, requestModel.OwnerName);

        _accountCommands.OpenAccount(financialAccount);
        
        createAccountOutput.Results(financialAccount.Id, financialAccount.Balance.Amount);
    }
}