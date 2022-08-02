using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.Exceptions;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public class AddIncomeTransactionUseCase : IAddIncomeTransactionInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly IAccountCommandsRepository _accountCommandsRepository;

    public AddIncomeTransactionUseCase(IAccountQueriesRepository accountQueriesRepository, IAccountCommandsRepository accountCommandsRepository)
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

        var accountBalance = account.AddIncomeTransaction(request.Amount, request.Description, request.Category);
        var transaction = account.Transactions.Items.First();
        
        var response = new AddTransactionResponse(transaction.Id, accountBalance.Amount);

        var addAccountModel = new AddTransactionModel(account.Id, accountBalance);
        
        _accountCommandsRepository.AddTransaction(addAccountModel, transaction);
        
        output.Results(response);
    }

    /// <exception cref="AccountNotFoundException"></exception>
    private FinancialAccount GetFinancialAccount(AddTransactionRequest request)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccount(request.AccountId, transactionsFilters);
        
        if (account == null) throw new AccountNotFoundException();
        
        return account;
    }
}