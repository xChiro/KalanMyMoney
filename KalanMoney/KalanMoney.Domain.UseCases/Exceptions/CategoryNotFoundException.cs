namespace KalanMoney.Domain.UseCases.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException() : base("Category cant by found.")
    { }
}