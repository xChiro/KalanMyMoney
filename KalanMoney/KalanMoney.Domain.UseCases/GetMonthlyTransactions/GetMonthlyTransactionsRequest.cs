namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record GetMonthlyTransactionsRequest(string AccountId, int Month, int Year);