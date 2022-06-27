namespace KalanMoney.Domain.UseCases.Common.Models;

public record AddTransactionResponse(string TransactionId, decimal AccountBalance, decimal CategoryBalance);