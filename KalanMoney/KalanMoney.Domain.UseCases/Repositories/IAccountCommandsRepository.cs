using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountCommandsRepository
{
    public void OpenAccount(FinancialAccount account);
}