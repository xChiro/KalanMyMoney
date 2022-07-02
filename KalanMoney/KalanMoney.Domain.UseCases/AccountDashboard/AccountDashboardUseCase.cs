using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Common.Exceptions;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public class AccountDashboardUseCase : IAccountDashboardInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;
    private readonly ICategoryQueriesRepository _categoryQueriesRepository;

    public AccountDashboardUseCase(IAccountQueriesRepository accountQueriesRepository,
        ICategoryQueriesRepository categoryQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
        _categoryQueriesRepository = categoryQueriesRepository;
    }

    /// <exception cref="AccountNotFoundException"></exception>
    public void Execute(string accountId, IAccountDashboardOutput output)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var accountTransactions = _accountQueriesRepository.GetTransactions(accountId, transactionsFilters);

        if (accountTransactions == null) throw new AccountNotFoundException();
        
        var categories = _categoryQueriesRepository.GetCategoriesOfAccount(accountId, transactionsFilters);
        CategoryBalanceModel[]? categoriesBalances = null;
        
        categoriesBalances = CreateCategoryBalanceModels(categories);
        
        var request = new AccountDashboardRequest(accountId, accountTransactions, categoriesBalances);
        
        output.Results(request);
    }

    private static CategoryBalanceModel[]? CreateCategoryBalanceModels(IReadOnlyList<FinancialCategory>? categories)
    {
        if (categories == null) return null;
        var categoriesBalances = new CategoryBalanceModel[categories.Count];

        for (int i = 0; i < categories.Count; i++)
        {
            var category = categories[i];
            categoriesBalances[i] = new CategoryBalanceModel(category.Id, category.Name.Value, category.Balance.Amount);
        }

        return categoriesBalances;
    }
}