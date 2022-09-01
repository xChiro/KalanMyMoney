using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccountWithoutTransactions(string id);

    public FinancialAccount? GetAccountByOwner(string ownerId, TransactionFilter transactionFilter);
    
    public Transaction[] GetMonthlyTransactions(string accountId, TransactionFilter transactionFilter);
}