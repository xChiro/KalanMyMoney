using KalanMoney.Domain.Entities.Exceptions;

namespace KalanMoney.Domain.Entities.ValueObjects;

public record AccountName
{
    private AccountName(string value)
    {
        Value = value;
    }
    
    public string Value { get; init; }

    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 50.
    /// </exception>
    public static AccountName Create(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > 50) throw new AccountNameException(name);

        return new AccountName(name);
    }
    
    /// <exception cref="AccountNameException">
    /// Name contains invalid values, is null or empty.
    /// Name lenght is greater than 50.
    /// </exception>
    public static AccountName Create(string? name, string alternative)
    {
        var finalName = string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) ? alternative : name;
        
        return Create(finalName);
    }
}