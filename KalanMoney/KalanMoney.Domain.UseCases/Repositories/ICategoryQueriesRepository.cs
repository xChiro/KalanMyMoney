using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryQueriesRepository
{
    FinancialCategory? GetCategoryById(string categoryName, TransactionFilters transactionFilters);
    FinancialCategory[]? GetCategoriesOfAccount(string accountId, TransactionFilters transactionFilters);
}