using System.Linq;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.GetCategoriesByAccount;

namespace KalanMoney.API.Functions.GetCategoriesByAccount;

public class GetCategoriesPresenter : IGetCategoriesByAccountOutput
{
    public string[] Categories { get; private set; }
    
    public void Results(Category[] categories)
    {
        Categories = categories.Select(x => x.Value).ToArray();
    }
}