using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.OpenAccount;

public class OpenAccountUseCase : IOpenAccountInput
{
    private readonly IAccountCommandsRepository _accountCommands;
    
    public OpenAccountUseCase(IAccountCommandsRepository accountCommands)
    {
        _accountCommands = accountCommands;
    }

    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 155.
    /// </exception>
    public void Execute(CreateAccountRequest requestModel, IOpenAccountOutput openAccountOutput)
    {
        var accountName = AccountName.Create(requestModel.AccountName, $"{requestModel.OwnerName} Account");
        var financialAccount = new FinancialAccount(accountName, requestModel.OwnerId, requestModel.OwnerName);

        _accountCommands.OpenAccount(financialAccount);

        var response = new OpenAccountResponse(financialAccount.Id, financialAccount.Balance.Amount);
        openAccountOutput.Results(response);
    }
}