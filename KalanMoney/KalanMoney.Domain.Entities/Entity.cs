namespace KalanMoney.Domain.Entities;

public abstract class Entity
{
    public string Id { get; init; }

    public Entity()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    public Entity(string id)
    {
        Id = id;
    }
}