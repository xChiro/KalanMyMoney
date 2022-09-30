using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccountWithoutTransactions(string id, string ownerId);

    public FinancialAccount? GetAccountByOwner(string ownerId, DateRangeFilter dateRangeFilter);
    
    public Transaction[] GetMonthlyTransactions(string accountId, string ownerId, GetTransactionsFilters filters);
    
    public Category[] GetCategoriesByAccount(string accountId, string ownerId);
}