using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.GetCategoriesByAccount;

namespace KalanMoney.Domain.UseCases.Tests.GetCategoriesByAccount;

public class GetCategoryByAccountOutputTest : IGetCategoriesByAccountOutput
{
    public Category[] Categories { get; private set; }
    
    public void Results(Category[] categories)
    {
        Categories = categories;
    }
}