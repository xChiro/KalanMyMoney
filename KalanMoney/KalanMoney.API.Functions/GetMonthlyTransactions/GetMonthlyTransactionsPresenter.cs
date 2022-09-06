using System.Linq;
using KalanMoney.API.Functions.Commons;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;

namespace KalanMoney.API.Functions.GetMonthlyTransactions;

public class GetMonthlyTransactionsPresenter : IGetMonthlyTransactionsOutput
{
    public TransactionResponse[] Transactions { get; private set; }
    
    public void Results(Transaction[] transactions)
    {
        Transactions = transactions.Select(x =>
                new TransactionResponse(x.Id, x.Amount, x.Description.Value, x.Category.Value,
                    x.TimeStamp.ToDateTime()))
            .ToArray();
    }
}