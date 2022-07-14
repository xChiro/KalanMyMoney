namespace KalanMoney.API.Functions.AddOutcomeTransaction;

public class AddOutcomeTransactionFunctionRequest
{
    public AddOutcomeTransactionFunctionRequest(string accountId, string categoryId, decimal amount)
    {
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
    }

    public string AccountId { get;}
    public decimal Amount { get; }
    public string CategoryId { get; }
}