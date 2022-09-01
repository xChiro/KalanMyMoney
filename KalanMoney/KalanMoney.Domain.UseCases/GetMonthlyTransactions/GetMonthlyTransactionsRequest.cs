namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record GetMonthlyTransactionsRequest(string AccountId, int Year, int Month);