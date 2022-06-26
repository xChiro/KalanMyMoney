namespace KalanMoney.Domain.UseCases.AddIncomeTransaction;

public interface IAddIncomeTransactionInput
{
    public void Execute(AddIncomeTransactionRequest request, IAddIncomeTransactionOutput output);
}