namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public record AddIncomeTransactionRequest(string AccountId, string CategoryId, decimal Amount);