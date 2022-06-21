namespace KalanMoney.Domain.UseCases.Adapters;

public interface ICreateAccountOutput
{
    public void Results(string accountId, decimal accountBalance);
}