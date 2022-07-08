using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class AccountsMemoryRepository : IAccountCommandsRepository
{
    public Dictionary<string, FinancialAccountModel> FinancialAccounts { get; }

    public AccountsMemoryRepository()
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>();
    }

    public AccountsMemoryRepository(FinancialAccountModel financialAccountModel)
    {
        FinancialAccounts = new Dictionary<string, FinancialAccountModel>()
            {{financialAccountModel.Id, financialAccountModel}};
    }

    public void OpenAccount(FinancialAccount account)
    {
        FinancialAccounts.Add(account.Id, FinancialAccountModel.CreateFromFinancialAccount(account));
    }

    public void AddTransaction(AddTransactionModel addTransactionModel, Transaction transaction)
    {
        FinancialAccounts[addTransactionModel.AccountId].Balance = addTransactionModel.AccountBalance.Amount;
        FinancialAccounts[addTransactionModel.AccountId].Transactions.Add(transaction);
        FinancialAccounts[addTransactionModel.AccountId].CategoryModels[addTransactionModel.CategoryId].Balance =
            addTransactionModel.CategoryBalance.Amount;
        FinancialAccounts[addTransactionModel.AccountId].CategoryModels[addTransactionModel.CategoryId].Transactions
            .Add(transaction);
    }

    public int ItemsCount()
    {
        return FinancialAccounts.Count;
    }
}