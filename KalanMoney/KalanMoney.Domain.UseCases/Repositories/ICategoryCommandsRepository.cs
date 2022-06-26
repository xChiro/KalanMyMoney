using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface ICategoryCommandsRepository
{
    public FinancialCategory CreateCategory(string name);
}