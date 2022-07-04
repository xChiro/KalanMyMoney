namespace KalanMoney.API.Functions.AccountFunctions;

public class OpenAccountFunctionRequest
{
    public string AccountName { get; init; }

    public OpenAccountFunctionRequest(string accountName)
    {
        AccountName = accountName;
    }
}