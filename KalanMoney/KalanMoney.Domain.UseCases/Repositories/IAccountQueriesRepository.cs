using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.Repositories;

public interface IAccountQueriesRepository
{
    public FinancialAccount? GetAccount(string id, TransactionFilter transactionFilter);

    public FinancialAccount? GetAccountByOwner(string ownerId, TransactionFilter transactionFilter);
    
    public Transaction[] GetMonthlyTransactions(string accountId, int invalidMonth, int year);
}