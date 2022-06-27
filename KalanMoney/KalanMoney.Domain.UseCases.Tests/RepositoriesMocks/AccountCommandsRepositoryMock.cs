using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;

public class AccountCommandsRepositoryMock : IAccountCommandsRepository
{
    private readonly Dictionary<string, FinancialAccount> _inAccountMemoryDb;

    public AddTransactionAccountModel ResultAccountModel { get; private set; }
    public AddTransactionCategoryModel ResultCategoryModel { get; private set; }
    public Transaction ResultTransaction { get; private set; }

    public AccountCommandsRepositoryMock(FinancialAccount firstFinancialAccount)
    {
        _inAccountMemoryDb = new Dictionary<string, FinancialAccount>();
        _inAccountMemoryDb.Add(firstFinancialAccount.Id, firstFinancialAccount);
    }

    public void OpenAccount(FinancialAccount account)
    {
        _inAccountMemoryDb.Add(account.Id, account);
    }

    public void AddTransaction(AddTransactionAccountModel addTransactionAccountModel, Transaction transaction,
        AddTransactionCategoryModel categoryModel)
    {
        ResultTransaction = transaction;
        ResultAccountModel = addTransactionAccountModel;
        ResultCategoryModel = categoryModel;
    }
} 