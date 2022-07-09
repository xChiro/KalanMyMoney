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
    private readonly ICategoryQueriesRepository _categoryQueriesRepository;
    private readonly IAccountCommandsRepository _accountCommandsRepository;

    public AddIncomeTransactionUseCase(IAccountQueriesRepository accountQueriesRepository, 
        ICategoryQueriesRepository categoryQueriesRepository, IAccountCommandsRepository accountCommandsRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _categoryQueriesRepository = categoryQueriesRepository;
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
        var category = GetFinancialCategory(request);

        var accountBalance = account.AddIncomeTransaction(request.Amount);
        var transaction = account.Transactions.Items.First();
        
        var categoryBalance= category.AddTransaction(transaction);
        var response = new AddTransactionResponse(transaction.Id, accountBalance.Amount, categoryBalance.Amount);

        var addAccountModel = new AddTransactionModel(account.Id, accountBalance, category.Id, categoryBalance);
        
        _accountCommandsRepository.AddTransaction(addAccountModel, transaction);
        
        output.Results(response);
    }

    /// <exception cref="CategoryNotFoundException"></exception>
    private FinancialCategory GetFinancialCategory(AddTransactionRequest request)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var category = _categoryQueriesRepository.GetCategoryById(request.CategoryId, transactionsFilters);
        
        if (category == null) throw new CategoryNotFoundException();
        
        return category;
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