namespace KalanMoney.Domain.UseCases.CreateCategory;

public interface ICreateCategoryInput
{
    public void Execute(CreateCategoryRequest request, ICreateCategoryOutput output);
}