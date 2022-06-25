namespace KalanMoney.Domain.UseCases.OpenAccount;

public class CreateAccountRequest
{
    public CreateAccountRequest(string ownerId, string ownerName, string accountName)
    {
        OwnerId = ownerId;
        OwnerName = ownerName;
        AccountName = accountName;
    }

    public string OwnerId { get; init; }

    public string OwnerName { get; init; }

    public string AccountName { get; init; }
}