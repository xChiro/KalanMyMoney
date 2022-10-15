using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;

namespace KalanMoney.Domain.UseCases.GetMonthlyTransactions;

public class GetMonthlyTransactions : IGetMonthlyTransactionsInput
{
    private readonly IAccountQueriesRepository _accountQueriesRepository;

    public GetMonthlyTransactions(IAccountQueriesRepository accountQueriesRepository)
    {
        _accountQueriesRepository = accountQueriesRepository;
    }

    public void Execute(GetMonthlyTransactionsRequest request, IGetMonthlyTransactionsOutput output)
    {
        var transactionFilter = DateRangeFilter.CreateMonthRange(request.Filters.Year, request.Filters.Month);
        var filters = new GetTransactionsFilters(transactionFilter,
            string.IsNullOrEmpty(request.Filters.Category)
                ? null
                : new[] {Category.Create(request.Filters.Category.ToLower())});

        var transactions =
            _accountQueriesRepository.GetMonthlyTransactions(request.AccountId, request.OwnerId, filters);

        output.Results(transactions);
    }
}