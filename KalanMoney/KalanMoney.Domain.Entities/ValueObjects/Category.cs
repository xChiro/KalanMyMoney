namespace KalanMoney.Domain.Entities.ValueObjects;

public record Category
{
    public string Value { get; }
    
    private Category(string value)
    {
        Value = value;
    }

    public static Category Create(string name)
    {
        var newName = name;
        if (newName.Length > 15) newName = newName[..15];

        return new Category(newName);
    }
}