namespace KalanMoney.API.Functions.OpenAccountFunctions;

public class OpenAccountFunctionRequest
{
    public string AccountName { get; init; }

    public OpenAccountFunctionRequest(string accountName)
    {
        AccountName = accountName;
    }
}