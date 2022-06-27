namespace KalanMoney.Domain.UseCases.Common.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base("Account Id Not Found.")
    { }
}