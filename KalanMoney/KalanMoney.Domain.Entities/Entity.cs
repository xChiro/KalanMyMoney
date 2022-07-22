namespace KalanMoney.Domain.Entities;

public abstract class Entity
{
    public string Id { get; init; }

    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

    protected Entity(string id)
    {
        Id = id;
    }
}