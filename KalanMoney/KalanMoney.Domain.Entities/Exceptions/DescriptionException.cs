namespace KalanMoney.Domain.Entities.Exceptions;

public class DescriptionException : Exception
{
    public DescriptionException() : base("Description is to long.")
    { }
}