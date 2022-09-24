using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.GetCategoriesByAccount;

public interface IGetCategoriesByAccountOutput
{
    public void Results(Category[] categories);
}