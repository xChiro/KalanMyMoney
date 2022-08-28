namespace KalanMoney.Domain.Entities.ValueObjects;

public record Category
{
    public string Value { get; }
    
    private Category(string value)
    {
        Value = value;
    }

    public static Category Create(string category)
    {
        if (string.IsNullOrEmpty(category)) throw new ArgumentNullException(nameof(category), "Category can't be null or empty");
        
        var newName = category;
        
        if (newName.Length > 15) newName = newName[..15];

        return new Category(newName.ToLower());
    }
}