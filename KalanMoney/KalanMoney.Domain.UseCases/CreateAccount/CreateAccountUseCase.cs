using KalanMoney.Domain.Entities;
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

    public void Execute(CreateAccountRequest requestModel, ICreateAccountOutput createAccountOutput)
    {
        var accountName = AccountName.Create(requestModel.AccountName);
        var financialAccount = new FinancialAccount(accountName, requestModel.OwnerId, requestModel.OwnerName);

        decimal balance;
        
        if (requestModel.OpeningTransaction >= 0)
            balance = financialAccount.AddIncomeTransaction(requestModel.OpeningTransaction);
        else
            balance = financialAccount.AddOutcomeTransaction(requestModel.OpeningTransaction);

        var category = CreateCategory(requestModel.CategoryName, financialAccount.Id, financialAccount.Owner,
            financialAccount.Transactions.Items[0]);

        _accountCommands.OpenAccount(financialAccount, category);
        
        createAccountOutput.Results(financialAccount.Id, balance);
    }

    private FinancialCategory CreateCategory(string? categoryName, string accountId, Owner owner, Transaction transaction)
    {
        var accountName = AccountName.Create(categoryName, "Salary");

        var category = new FinancialCategory(accountName, accountId, owner);
        category.AddTransaction(transaction);

        return category;
    }
}