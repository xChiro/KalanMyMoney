namespace KalanMoney.Domain.UseCases.Common.Models;

public record AddTransactionRequest(string AccountId, decimal Amount, string Description, string Category);