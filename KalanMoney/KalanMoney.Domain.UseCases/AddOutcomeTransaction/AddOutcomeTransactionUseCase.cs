using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.AddOutcomeTransaction;

public class AddOutcomeTransactionUseCase : IAddOutcomeTransactionInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly IAccountCommandsRepository _accountCommandsRepository;

    public AddOutcomeTransactionUseCase(IAccountQueriesRepository accountQueriesRepository,
        IAccountCommandsRepository accountCommandsRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _accountCommandsRepository = accountCommandsRepository;
    }

    public void Execute(AddTransactionRequest request, IAddOutcomeTransactionOutput output)
    {
        var account = GetFinancialAccount(request);
        
        if (account == null) throw new AccountNotFoundException();

        var newAccountBalance = account.AddOutcomeTransaction(request.Amount, request.Description, request.Category);
        var transaction = account.Transactions.GetLastTransaction()!;
        
        _accountCommandsRepository.StoreTransaction(account.Id, newAccountBalance, transaction);

        var response = new AddTransactionResponse(transaction.Id, newAccountBalance.Amount);
        output.Results(response);
    }
    
    private FinancialAccount? GetFinancialAccount(AddTransactionRequest request)
    {
        var account = _accountQueriesRepository.GetAccountWithoutTransactions(request.AccountId, request.OwnerId);
        
        return account;
    }
}