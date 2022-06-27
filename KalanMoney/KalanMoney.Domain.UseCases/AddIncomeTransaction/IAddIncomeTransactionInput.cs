using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public interface IAddIncomeTransactionInput
{
    public void Execute(AddTransactionRequest request, IAddIncomeTransactionOutput output);
}