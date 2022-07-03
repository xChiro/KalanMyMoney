using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;

namespace KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;

public class CategoryCommandsRepositoryMock : ICategoryCommandsRepository
{
    public CategoryCommandsRepositoryMock()
    {
        Categories = new Dictionary<string, FinancialCategory>();
    }

    public Dictionary<string, FinancialCategory> Categories { get; private set; }

    public void CreateCategory(FinancialCategory category)
    {
        Categories.Add(category.Id, category);
    }
}