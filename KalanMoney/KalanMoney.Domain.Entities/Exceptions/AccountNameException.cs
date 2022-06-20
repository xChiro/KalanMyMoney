namespace KalanMoney.Domain.Entities.Exceptions;

public class AccountNameException : Exception
{
    public string invalidName { get; }

    public AccountNameException(string invalidName)
    {
        
    }
}