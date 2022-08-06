using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;

public class AccountCommandsRepositoryMock : IAccountCommandsRepository
{
    public Transaction ResultTransaction { get; private set; }

    public string AccountId { get; private set; }
    
    public Balance Balance { get; private set; }
    
    public FinancialAccount FinancialAccount { get; private set; }

    public void OpenAccount(FinancialAccount account)
    {
        FinancialAccount = account;
    }

    public void AddTransaction(string accountId, Balance balance, Transaction transaction)
    {
        ResultTransaction = transaction;
        Balance = balance;
        AccountId = accountId;
    }
} 