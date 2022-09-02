namespace KalanMoney.Domain.UseCases.Common.Models;

public record AddTransactionRequest(string AccountId, string OwnerId, decimal Amount, string Description, string Category);