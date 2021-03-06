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
    public void Execute(string ownerId, IAccountDashboardOutput output)
    {
        var transactionsFilters = TransactionFilter.CreateMonthRangeFromUtcNow();
        var account = _accountQueriesRepository.GetAccountByOwner(ownerId, transactionsFilters);

        if (account == null) throw new AccountNotFoundException();
        
        var categories = _categoryQueriesRepository.GetCategoriesFromAccount(ownerId, transactionsFilters);
        CategoryBalanceModel[]? categoriesBalances = null;
        
        categoriesBalances = CreateCategoryBalanceModels(categories);
        
        var request = new AccountDashboardResponse(account.Id, account.Name.Value, account.Transactions.Items, categoriesBalances);
        
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