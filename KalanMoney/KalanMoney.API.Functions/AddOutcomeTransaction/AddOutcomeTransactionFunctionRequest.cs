namespace KalanMoney.API.Functions.AddOutcomeTransaction;

public class AddOutcomeTransactionFunctionRequest
{
    public AddOutcomeTransactionFunctionRequest(string accountId, string categoryId, decimal amount, string transactionDescription)
    {
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        TransactionDescription = transactionDescription;
    }

    public string AccountId { get;}
    public decimal Amount { get; }
    public string CategoryId { get; }
    public string TransactionDescription { get; }
}