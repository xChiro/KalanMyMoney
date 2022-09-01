using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;

namespace KalanMoney.API.Functions.GetMonthlyTransactions;

public class GetMonthlyTransactionsPresenter : IGetMonthlyTransactionsOutput
{
    public Transaction[] Transactions { get; private set; }
    
    public void Results(Transaction[] transactions)
    {
        Transactions = transactions;
    }
}