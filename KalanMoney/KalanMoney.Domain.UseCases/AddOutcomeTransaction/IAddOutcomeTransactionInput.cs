using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.Domain.UseCases.AddOutcomeTransaction;

public interface IAddOutcomeTransactionInput
{
    public void Execute(AddTransactionRequest addTransactionRequest, IAddOutcomeTransactionOutput output);
}