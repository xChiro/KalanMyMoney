namespace KalanMoney.Domain.Entities.Exceptions;

public class AccountNameException : Exception
{
    public string InvalidName { get; }

    public AccountNameException(string invalidName) : base("Account name is invalid.")
    {
        InvalidName = invalidName;
    }
}