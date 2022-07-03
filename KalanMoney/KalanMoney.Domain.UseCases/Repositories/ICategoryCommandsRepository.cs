using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryCommandsRepository
{
    public void CreateCategory(FinancialCategory category);
}