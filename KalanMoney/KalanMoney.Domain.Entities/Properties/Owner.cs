namespace KalanMoney.Domain.Entities.Properties;

public record Owner(string ExternalUserId, string Name)
{
    public string Name { get; set; } = Name;
}