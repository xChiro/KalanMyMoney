namespace KalanMoney.Domain.UseCases.CreateAccount;

public class CreateAccountRequest
{
    public CreateAccountRequest(string ownerId, string ownerName, string accountName, decimal openingTransaction, 
        string? categoryName = null)
    {
        OwnerId = ownerId;
        OwnerName = ownerName;
        AccountName = accountName;
        OpeningTransaction = openingTransaction;
        CategoryName = categoryName;
    }

    public string OwnerId { get; init; }

    public string OwnerName { get; init; }

    public string AccountName { get; init; }
    
    public decimal OpeningTransaction { get; init; }
    
    public string? CategoryName { get; init; }

}