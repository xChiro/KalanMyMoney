namespace KalanMoney.Domain.Entities.ValueObjects;

public record Balance(decimal Amount)
{
    public Balance Add(decimal amount)
    {
        var newBalance = Amount + amount;
        
        return new Balance(newBalance);
    }
}