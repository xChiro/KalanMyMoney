namespace KalanMoney.Domain.Entities.ValueObjects;

public record Description 
{
    public string Value { get;  }

    private Description(string value)
    {
        Value = value;
    }

    /// <exception cref="ArgumentNullException">
    /// Description can't be null or empty.
    /// </exception>
    public static Description Create(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException(nameof(description), "Description can't be null or empty");
        
        var newDescription = description;
        if (newDescription.Length > 155) newDescription = newDescription[..155];

        return new Description(newDescription);
    }
}