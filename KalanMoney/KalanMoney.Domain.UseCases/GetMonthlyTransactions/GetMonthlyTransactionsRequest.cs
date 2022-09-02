namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record GetMonthlyTransactionsRequest(string AccountId, string OwnerId, int Year, int Month);