using KalanMoney.Domain.Entities.Exceptions;

namespace KalanMoney.Domain.Entities.Properties;

public record AccountName
{
    private AccountName(string value)
    {
        Value = value;
    }
    
    public string Value { get; init; }

    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 155.
    /// </exception>
    public static AccountName Create(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > 155) throw new AccountNameException(name);

        return new AccountName(name);
    }
    
    public static AccountName Create(string? name, string alternative)
    {
        if (!string.IsNullOrEmpty(name) && name.Length > 155) throw new AccountNameException(name);

        return new AccountName(name ?? alternative);
    }
}