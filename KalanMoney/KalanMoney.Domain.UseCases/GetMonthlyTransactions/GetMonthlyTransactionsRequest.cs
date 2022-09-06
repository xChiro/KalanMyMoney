namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record GetMonthlyTransactionsRequest(string AccountId, string OwnerId, TransactionsFilters Filters);