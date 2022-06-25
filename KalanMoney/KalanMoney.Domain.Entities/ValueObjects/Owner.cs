namespace KalanMoney.Domain.Entities.ValueObjects;

public record Owner(string ExternalUserId, string Name)
{
    public string Name { get; set; } = Name;
}