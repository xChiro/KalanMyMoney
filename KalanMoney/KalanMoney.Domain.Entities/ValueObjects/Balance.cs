namespace KalanMoney.Domain.Entities.ValueObjects;

public record Balance(decimal Amount = 0)
{
    public Balance SumAmount(decimal amount)
    {
        var newBalanceAmount = Amount + amount;
        return new Balance(newBalanceAmount);
    }
}