using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountCommandsRepository
{
    public void OpenAccount(FinancialAccount account);

    public void AddTransaction(string accountId, Balance accountBalance, Transaction transaction);
}