namespace KalanMoney.Domain.Entities.ValueObjects;

public record Balance(decimal Amount)
{
    public Balance Add(decimal amount)
    {
        var newBalanceAmount = Amount + amount;
        
        return new Balance(newBalanceAmount);
    }
}