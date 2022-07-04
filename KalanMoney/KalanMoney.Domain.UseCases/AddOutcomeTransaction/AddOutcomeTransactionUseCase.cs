using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Common.Models;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.AddOutcomeTransaction;

public class AddOutcomeTransactionUseCase : IAddOutcomeTransactionInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly ICategoryQueriesRepository _categoryQueriesRepository;
    private readonly IAccountCommandsRepository _accountCommandsRepository;

    public AddOutcomeTransactionUseCase(IAccountQueriesRepository accountQueriesRepository,
        ICategoryQueriesRepository categoryQueriesRepository, 
        IAccountCommandsRepository accountCommandsRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _categoryQueriesRepository = categoryQueriesRepository;
        _accountCommandsRepository = accountCommandsRepository;
    }

    public void Execute(AddTransactionRequest addTransactionRequest, IAddOutcomeTransactionOutput output)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccount(addTransactionRequest.AccountId, transactionsFilters);
        var category = _categoryQueriesRepository.GetCategoryById(addTransactionRequest.CategoryId, transactionsFilters);

        if (account == null) throw new AccountNotFoundException();
        if (category == null) throw new CategoryNotFoundException();

        var accountBalance = account.AddOutcomeTransaction(addTransactionRequest.Amount);
        var transaction = account.Transactions.Items.First();
        
        var categoryBalance = category.AddTransaction(transaction);
        
        var addAccountModel = new AddTransactionAccountModel(account.Id, accountBalance);
        var addCategoryModel = new AddTransactionCategoryModel(category.Id, categoryBalance);

        _accountCommandsRepository.AddTransaction(addAccountModel, transaction, addCategoryModel);
        
        output.Results(new AddTransactionResponse(transaction.Id, accountBalance.Amount, categoryBalance.Amount));
    }
}