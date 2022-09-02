using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public class GetMonthlyTransactionsUseCase : IGetMonthlyTransactionsInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public GetMonthlyTransactionsUseCase(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    public void Execute(GetMonthlyTransactionsRequest request, IGetMonthlyTransactionsOutput output)
    {
        if (request.Month is < 1 or > 12) throw new IndexOutOfRangeException("Invalid month number");
        if (request.Year < DateTime.MinValue.Year || request.Year > DateTime.MaxValue.Year)
            throw new IndexOutOfRangeException("Invalid year number");

        var transactionFilter = TransactionFilter.CreateMonthRange(request.Year, request.Month);
        
        var transactions =
            _accountQueriesRepository.GetMonthlyTransactions(request.AccountId, request.OwnerId, transactionFilter);
        
        output.Results(transactions);
    }
}