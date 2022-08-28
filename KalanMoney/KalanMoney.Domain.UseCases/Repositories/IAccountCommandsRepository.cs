using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountCommandsRepository
{
    public void OpenAccount(FinancialAccount account);

    public void StoreTransaction(string accountId, Balance accountBalance, Transaction transaction);
}