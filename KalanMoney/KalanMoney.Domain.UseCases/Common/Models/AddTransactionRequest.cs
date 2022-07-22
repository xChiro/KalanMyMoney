namespace KalanMoney.Domain.UseCases.Common.Models;

public record AddTransactionRequest(string AccountId, string CategoryId, decimal Amount, string Description);