using KalanMoney.Domain.UseCases.CreateCategory;

namespace KalanMoney.Domain.UseCases.Tests.CreateCategoryTests;

public class CreateCategoryOutputMock : ICreateCategoryOutput
{
    public string CategoryId { get; private set; }
    
    public void Response(string categoryId)
    {
        CategoryId = categoryId;
    }
}