namespace KalanMoney.Domain.UseCases.OpenAccount;

public interface IOpenAccountOutput
{
    public void Results(string accountId, decimal accountBalance);
}