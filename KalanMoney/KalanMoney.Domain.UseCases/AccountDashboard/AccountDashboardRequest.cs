using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.AccountDashboard;

public record AccountDashboardRequest(string AccountId, Transaction[]? AccountTransactions, CategoryBalanceModel[]? CategoryBalanceModels);