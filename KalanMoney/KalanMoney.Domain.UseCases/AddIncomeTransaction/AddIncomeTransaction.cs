using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public class AddIncomeTransaction : IAddIncomeTransactionInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly IAccountCommandsRepository _accountCommandsRepository;

    public AddIncomeTransaction(IAccountQueriesRepository accountQueriesRepository, IAccountCommandsRepository accountCommandsRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _accountCommandsRepository = accountCommandsRepository;
    }

    /// <exception cref="CategoryNotFoundException"></exception>
    /// <exception cref="AccountNotFoundException"></exception>
    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 155.
    /// </exception>
    public void Execute(AddTransactionRequest request, IAddIncomeTransactionOutput output)
    {
        var account = GetFinancialAccount(request);

        if (account == null) throw new AccountNotFoundException();
        
        var newAccountBalance = account.AddIncomeTransaction(request.Amount, request.Description, request.Category);
        var transaction = account.Transactions.GetLastTransaction()!;

        _accountCommandsRepository.StoreTransaction(account.Id, newAccountBalance, transaction);

        var response = new AddTransactionResponse(transaction.Id, newAccountBalance.Amount);
        output.Results(response);
    }

    /// <exception cref="AccountNotFoundException"></exception>
    private FinancialAccount? GetFinancialAccount(AddTransactionRequest request)
    {
        var account = _accountQueriesRepository.GetAccountWithoutTransactions(request.AccountId, request.OwnerId);
        
        return account;
    }
}