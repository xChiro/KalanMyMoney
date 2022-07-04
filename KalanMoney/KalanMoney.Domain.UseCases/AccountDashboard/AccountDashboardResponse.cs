using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public record AccountDashboardResponse(string AccountId, string AccountName, Transaction[]? AccountTransactions, CategoryBalanceModel[]? CategoryBalanceModels);