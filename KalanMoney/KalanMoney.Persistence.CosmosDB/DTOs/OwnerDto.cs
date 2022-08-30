namespace KalanMoney.Persistence.CosmosDB.DTOs;

public class OwnerDto
{
    public OwnerDto(string subId, string name)
    {
        SubId = subId;
        Name = name;
    }
    
    public string SubId { get; }

    public string Name { get; }
    
}