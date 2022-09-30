using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.MemoryDatabase.DTOs;

namespace KalanMoney.Persistence.MemoryDatabase;

public class AccountsMemoryRepository : IAccountCommandsRepository, IAccountQueriesRepository
{
    public MemoryDb DataBase { get; }

    public AccountsMemoryRepository() : this(new MemoryDb())
    {
    }

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

    public FinancialAccount? GetAccountWithoutTransactions(string id, string ownerId)
    {
        if (!DataBase.FinancialAccounts.TryGetValue(id, out var financialAccountModel)) return null;

        return financialAccountModel.OwnerId != ownerId ? null : financialAccountModel.ToFinancialAccount();
    }

    public FinancialAccount? GetAccountByOwner(string ownerId, DateRangeFilter dateRangeFilter)
    {
        var accountModel = DataBase.FinancialAccounts.Where(keyValuePair => keyValuePair.Value.OwnerId == ownerId)
            .Select(keyValuePair => keyValuePair.Value).FirstOrDefault();

        if (accountModel == null) return null;

        var transactions = ApplyFilters(dateRangeFilter, accountModel);
        return FinancialAccountModel.ToFinancialAccount(accountModel, transactions);
    }

    public Transaction[] GetMonthlyTransactions(string accountId, string ownerId,
        GetTransactionsFilters dateRangeFilter)
    {
        var accountModel = DataBase.FinancialAccounts.Where(valuePair => valuePair.Value.Id == accountId
                                                                         && valuePair.Value.OwnerId == ownerId)
            .Select(keyValuePair => keyValuePair.Value).FirstOrDefault();

        if (accountModel == null) throw new KeyNotFoundException("Account id not found.");

        var transactions = ApplyFilters(dateRangeFilter, accountModel);

        return transactions.ToArray();
    }

    public Category[] GetCategoriesByAccount(string accountId, string ownerId)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<Transaction> ApplyFilters(DateRangeFilter filters,
        FinancialAccountModel financialAccountModel)
    {
        bool Predicate(Transaction x) => x.TimeStamp.ToDateTime() >= filters.From.ToDateTime(TimeOnly.MinValue) &&
                                         x.TimeStamp.ToDateTime() <= filters.To.ToDateTime(TimeOnly.MaxValue);

        return ApplyFilters(Predicate, financialAccountModel);
    }

    private static IEnumerable<Transaction> ApplyFilters(GetTransactionsFilters filters,
        FinancialAccountModel financialAccountModel)
    {
        bool Predicate(Transaction x) =>
            x.TimeStamp.ToDateTime() >= filters.RangeFilter.From.ToDateTime(TimeOnly.MinValue) &&
            x.TimeStamp.ToDateTime() <= filters.RangeFilter.To.ToDateTime(TimeOnly.MaxValue) &&
            (filters.Categories == null || filters.Categories.Contains(x.Category));

        return ApplyFilters(Predicate, financialAccountModel);
    }

    private static IEnumerable<Transaction> ApplyFilters(Func<Transaction, bool> filters,
        FinancialAccountModel financialAccountModel)
    {
        return financialAccountModel.Transactions.Where(filters);
    }

    public int ItemsCount()
    {
        return DataBase.FinancialAccounts.Count;
    }
}