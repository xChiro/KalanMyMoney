namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public record TransactionsFilters(int Year, int Month, string? Category = null);