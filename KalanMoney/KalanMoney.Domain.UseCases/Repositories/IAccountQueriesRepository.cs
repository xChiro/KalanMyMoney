using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccountById(string id);
    
    Transaction[]? GetTransactions(string accountId, DateOnly from, DateOnly to);
}