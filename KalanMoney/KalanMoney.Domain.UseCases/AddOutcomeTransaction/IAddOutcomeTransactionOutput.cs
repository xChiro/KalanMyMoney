using KalanMoney.Domain.UseCases.Common.Models;

namespace KalanMoney.Domain.UseCases.AddOutcomeTransaction;

public interface IAddOutcomeTransactionOutput
{
    public void Results(AddTransactionResponse response);
}