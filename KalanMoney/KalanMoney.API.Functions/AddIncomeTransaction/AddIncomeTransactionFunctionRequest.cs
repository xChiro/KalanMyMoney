namespace KalanMoney.API.Functions.AddIncomeTransaction;

public record AddIncomeTransactionFunctionRequest(string AccountId, decimal Amount, string Category, string TransactionDescription);