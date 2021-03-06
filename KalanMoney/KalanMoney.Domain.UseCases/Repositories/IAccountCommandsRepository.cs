using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountCommandsRepository
{
    public void OpenAccount(FinancialAccount account);

    public void AddTransaction(AddTransactionModel addTransactionModel, Transaction transaction);
}