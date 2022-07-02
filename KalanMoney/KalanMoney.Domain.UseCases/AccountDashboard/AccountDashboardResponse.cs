using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public record AccountDashboardResponse(string AccountId, Transaction[]? AccountTransactions, CategoryBalanceModel[]? CategoryBalanceModels);