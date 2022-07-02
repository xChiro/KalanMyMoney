using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryQueriesRepository
{
    FinancialCategory? GetCategoryById(string categoryName, TransactionFilter transactionFilter);
    FinancialCategory[]? GetCategoriesOfAccount(string accountId, TransactionFilter transactionFilter);
}