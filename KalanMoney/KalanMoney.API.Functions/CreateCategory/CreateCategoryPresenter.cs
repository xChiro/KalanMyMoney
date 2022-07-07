using KalanMoney.Domain.UseCases.CreateCategory;

namespace KalanMoney.API.Functions.CreateCategory;

public record CreateCategoryPresenter : ICreateCategoryOutput
{
    public string CategoryId { get; private set; }
    
    public void Response(string categoryId)
    {
        CategoryId = categoryId;
    }
}