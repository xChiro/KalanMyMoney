using KalanMoney.Domain.Entities;

namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public interface IGetMonthlyTransactionsOutput
{
    public void Results(Transaction[] transactions);
}