using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;

public class AccountCommandsRepositoryMock : IAccountCommandsRepository
{
    private readonly Dictionary<string, FinancialAccount> _inAccountMemoryDb;

    public AddTransactionModel ResultModel { get; private set; }
    public Transaction ResultTransaction { get; private set; }

    public AccountCommandsRepositoryMock()
    {
        _inAccountMemoryDb = new Dictionary<string, FinancialAccount>();
    }
    
    public AccountCommandsRepositoryMock(FinancialAccount firstFinancialAccount)
    {
        _inAccountMemoryDb = new Dictionary<string, FinancialAccount>();
        _inAccountMemoryDb.Add(firstFinancialAccount.Id, firstFinancialAccount);
    }

    public void OpenAccount(FinancialAccount account)
    {
        _inAccountMemoryDb.Add(account.Id, account);
    }

    public void AddTransaction(AddTransactionModel addTransactionModel, Transaction transaction)
    {
        ResultTransaction = transaction;
        ResultModel = addTransactionModel;
    }
} 