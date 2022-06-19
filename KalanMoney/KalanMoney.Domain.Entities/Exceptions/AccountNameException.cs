namespace KalanMoney.Domain.UseCases.Exceptions;

public class AccountNameException : Exception
{
    public string invalidName { get; }

    public AccountNameException(string invalidName)
    {
        
    }
}