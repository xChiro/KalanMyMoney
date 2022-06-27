using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public interface IAddIncomeTransactionOutput
{
    public void Results(AddTransactionResponse response);
}