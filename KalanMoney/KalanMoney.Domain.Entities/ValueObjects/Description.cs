using KalanMoney.Domain.Entities.Exceptions;

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
        if (description.Length > 155) throw new DescriptionException();

        return new Description(description);
    }
}