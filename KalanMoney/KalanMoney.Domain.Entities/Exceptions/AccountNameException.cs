namespace KalanMoney.Domain.Entities.Exceptions;

public class AccountNameException : Exception
{
    public string InvalidName { get; }

    public AccountNameException(string invalidName)
    {
        InvalidName = invalidName;
    }
}