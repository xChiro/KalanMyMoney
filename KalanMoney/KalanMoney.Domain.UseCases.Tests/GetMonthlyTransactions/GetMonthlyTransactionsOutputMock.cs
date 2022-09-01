using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;

namespace KalanMoney.Domain.UseCases.Tests.GetMonthlyTransactions;

public class GetMonthlyTransactionsOutputMock : IGetMonthlyTransactionsOutput
{
    public Transaction[] Transactions { get; private set; }
    
    public void Results(Transaction[] transactions)
    {
        Transactions = transactions;
    }
}