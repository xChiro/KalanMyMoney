namespace KalanMoney.API.Functions.AddIncomeTransaction;

public record AddIncomeTransactionFunctionRequest(string AccountId, string CategoryId, decimal Amount);