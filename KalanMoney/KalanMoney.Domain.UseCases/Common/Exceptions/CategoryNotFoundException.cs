namespace KalanMoney.Domain.UseCases.Common.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException() : base("Category cant by found.")
    { }
}