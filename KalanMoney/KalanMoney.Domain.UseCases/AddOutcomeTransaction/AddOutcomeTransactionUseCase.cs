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
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccount(request.AccountId, transactionsFilters);

        if (account == null) throw new AccountNotFoundException();

        var accountBalance = account.AddOutcomeTransaction(request.Amount, request.Description, request.Category);
        var transaction = account.Transactions.Items.First();
        
        
        var addAccountModel = new AddTransactionModel(account.Id, accountBalance);

        _accountCommandsRepository.AddTransaction(addAccountModel, transaction);
        
        output.Results(new AddTransactionResponse(transaction.Id, accountBalance.Amount));
    }
}