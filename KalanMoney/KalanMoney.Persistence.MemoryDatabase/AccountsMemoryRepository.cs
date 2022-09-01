using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class AccountsMemoryRepository : IAccountCommandsRepository, IAccountQueriesRepository
{
    public MemoryDb DataBase { get; }

    public AccountsMemoryRepository() : this(new MemoryDb()) { }

    public AccountsMemoryRepository(MemoryDb memoryDb)
    {
        DataBase = memoryDb;
    }

    public AccountsMemoryRepository(FinancialAccountModel financialAccountModel)
    {
        DataBase = new MemoryDb(financialAccountModel);
    }

    public void OpenAccount(FinancialAccount account)
    {
        DataBase.FinancialAccounts.Add(account.Id, FinancialAccountModel.CreateFromFinancialAccount(account));
    }

    public void StoreTransaction(string accountId, Balance accountBalance, Transaction transaction)
    {
        DataBase.FinancialAccounts[accountId].Balance = accountBalance.Amount;
        DataBase.FinancialAccounts[accountId].Transactions.Add(transaction);
    }

    public FinancialAccount? GetAccount(string id, TransactionFilter transactionFilter)
    {
        if (! DataBase.FinancialAccounts.TryGetValue(id, out var financialAccountModel)) return null;

        var transactions = ApplyFilters(transactionFilter, financialAccountModel);
        return  FinancialAccountModel.ToFinancialAccount(financialAccountModel, transactions);
    }

    public FinancialAccount? GetAccountByOwner(string ownerId, TransactionFilter transactionFilter)
    {
        var accountModel =  DataBase.FinancialAccounts.Where(x => x.Value.OwnerId == ownerId)
            .Select(x => x.Value).FirstOrDefault();

        if (accountModel == null) return null;
        
        var transactions = ApplyFilters(transactionFilter, accountModel);
        return FinancialAccountModel.ToFinancialAccount(accountModel, transactions);

    }

    public Transaction[] GetMonthlyTransactions(string accountId, TransactionFilter transactionFilter)
    {
        var accountModel =  DataBase.FinancialAccounts.Where(x => x.Value.Id == accountId)
            .Select(x => x.Value).FirstOrDefault();
        
        if (accountModel == null) throw new KeyNotFoundException("Account id not found.");
        
        var transactions = ApplyFilters(transactionFilter, accountModel);
        
        return transactions.ToArray();
    }

    private static IEnumerable<Transaction> ApplyFilters(TransactionFilter transactionFilter, FinancialAccountModel financialAccountModel)
    {
        return financialAccountModel.Transactions.Where(x =>
            x.TimeStamp.ToDateTime() >= transactionFilter.From.ToDateTime(TimeOnly.MinValue) &&  
            x.TimeStamp.ToDateTime() <= transactionFilter.To.ToDateTime(TimeOnly.MaxValue));
    }

    public int ItemsCount()
    {
        return  DataBase.FinancialAccounts.Count;
    }
}