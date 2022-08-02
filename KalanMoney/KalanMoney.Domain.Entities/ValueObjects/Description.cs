namespace KalanMoney.Domain.Entities.ValueObjects;

public struct Description 
{
    public string Value { get;  }

    private Description(string value)
    {
        Value = value;
    }

    public static Description Create(string description)
    {
        var newDescription = description;
        if (newDescription.Length > 155) newDescription = newDescription[..155];

        return new Description(newDescription);
    }
}