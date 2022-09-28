namespace KalanMoney.Domain.UseCases.GetCategoriesByAccount;

public interface IGetCategoriesByAccountInput
{
    public void Execute(string accountId, string ownerId, IGetCategoriesByAccountOutput output);
}