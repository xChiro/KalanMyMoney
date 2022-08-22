namespace KalanMoney.Domain.UseCases.OpenAccount;

public record CreateAccountRequest(string OwnerId, string OwnerName, string? AccountName);