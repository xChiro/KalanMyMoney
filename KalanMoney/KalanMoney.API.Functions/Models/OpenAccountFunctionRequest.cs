namespace KalanMoney.API.Functions.Models;

public class OpenAccountFunctionRequest
{
    public string AccountName { get; init; }

    public OpenAccountFunctionRequest(string accountName)
    {
        AccountName = accountName;
    }
}