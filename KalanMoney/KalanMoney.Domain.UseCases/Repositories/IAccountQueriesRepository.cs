using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccountById(string id, TransactionFilter transactionFilter);

    public bool AccountExists(string id);
    
    Transaction[]? GetTransactions(string accountId, TransactionFilter transactionFilter);
}