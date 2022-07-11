using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryQueriesRepository
{
    FinancialCategory? GetCategoryById(string categoryId, TransactionFilter transactionFilter);
    FinancialCategory[]? GetCategoriesFromAccount(string accountId, TransactionFilter transactionFilter);
}