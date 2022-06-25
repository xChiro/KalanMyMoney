namespace KalanMoney.Domain.Entities.ValueObjects;

public record Balance(decimal Amount = 0)
{
    public decimal Amount { get; private set; } = Amount;

    public decimal SumAmount(decimal amount)
    {
        Amount += amount;
        return Amount;
    }
}