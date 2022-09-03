using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccountWithoutTransactions(string id, string ownerId);

    public FinancialAccount? GetAccountByOwner(string ownerId, DateRangeFilter dateRangeFilter);
    
    public Transaction[] GetMonthlyTransactions(string accountId, string ownerId,  DateRangeFilter dateRangeFilter);
}