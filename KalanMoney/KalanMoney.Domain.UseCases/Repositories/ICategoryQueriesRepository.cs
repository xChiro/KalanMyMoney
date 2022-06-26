using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryQueriesRepository
{
    FinancialCategory? GetCategoryById(string categoryName);
}