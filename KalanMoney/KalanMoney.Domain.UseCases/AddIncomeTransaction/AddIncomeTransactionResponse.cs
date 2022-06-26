namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public record AddIncomeTransactionResponse(string TransactionId, decimal AccountBalance, decimal CategoryBalance);