namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public interface IGetMonthlyTransactionsInput
{
    public void Execute(GetMonthlyTransactionsRequest request, IGetMonthlyTransactionsOutput output);
}