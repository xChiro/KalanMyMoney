namespace KalanMoney.API.Functions.AddOutcomeTransaction;

public record AddOutcomeTransactionFunctionRequest(string AccountId, decimal Amount, string Category, string TransactionDescription);