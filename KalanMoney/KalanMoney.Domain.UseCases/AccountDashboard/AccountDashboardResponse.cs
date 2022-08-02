using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public record AccountDashboardResponse(string AccountId, string AccountName, Transaction[]? AccountTransactions,
    Dictionary<string, decimal> CategoriesBalances);