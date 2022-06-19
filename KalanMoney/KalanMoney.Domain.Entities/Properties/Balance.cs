namespace KalanMoney.Domain.Entities.Properties;

public record Balance(decimal Amount)
{
    public decimal Amount { get; private set; } = Amount;

    public decimal SumAmount(decimal amount)
    {
        Amount += amount;
        return Amount;
    }
}