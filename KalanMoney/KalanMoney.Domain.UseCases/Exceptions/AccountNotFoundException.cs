namespace KalanMoney.Domain.UseCases.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base("Account Id Not Found.")
    { }
}