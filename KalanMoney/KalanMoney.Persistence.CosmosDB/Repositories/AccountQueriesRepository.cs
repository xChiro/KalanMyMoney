using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Domain.UseCases.Repositories.Models;
using KalanMoney.Persistence.CosmosDB.DTOs;
using Microsoft.Azure.Cosmos;

namespace KalanMoney.Persistence.CosmosDB.Repositories;

public class AccountQueriesRepository : IAccountQueriesRepository
{
    private readonly Container _container;
    private readonly TaskFactory _taskFactory;

    public AccountQueriesRepository(Container container)
    {
        _container = container;
        _taskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);
    }

    public FinancialAccount? GetAccountWithoutTransactions(string id, string ownerId)
    {
        const string sqlQuery = $"SELECT c.id, c.name, c.owner, c.balance, c.creationTimeStamp, " +
                                $"ARRAY_SLICE(c.transactions, 1) as transactions " +
                                $"FROM c WHERE c.id = @idParam AND c.owner.subId = @ownerId";

        var queryDefinition = new QueryDefinition(sqlQuery)
            .WithParameter("@idParam", id)
            .WithParameter("@ownerId", ownerId);

        var queryRequestOptions = new QueryRequestOptions();

        using var feedIterator =
            _container.GetItemQueryIterator<FinancialAccountDto>(queryDefinition, null, queryRequestOptions);

        if (!feedIterator.HasMoreResults) throw new KeyNotFoundException("Account.Id not found.");

        var result = _taskFactory
            .StartNew(() => feedIterator.ReadNextAsync())
            .Unwrap()
            .GetAwaiter()
            .GetResult()
            .FirstOrDefault();

        return result?.ToFinancialAccount();
    }

    public FinancialAccount? GetAccountByOwner(string ownerId, DateRangeFilter dateRangeFilter)
    {
        const string sqlQuery = $"SELECT c.id, c.name, c.owner, c.balance, c.creationTimeStamp, " +
                                $"ARRAY(SELECT * FROM c JOIN t IN c.transactions " +
                                $"WHERE c.owner.subId = @ownerId " +
                                $"AND TimestampToDateTime(t.timeStamp) >= @from " +
                                $"AND TimestampToDateTime(t.timeStamp) <= @to) " +
                                $"as transactions FROM c WHERE c.owner.subId = @ownerId";

        var queryDefinition = new QueryDefinition(sqlQuery)
            .WithParameter("@ownerId", ownerId)
            .WithParameter("@from", dateRangeFilter.From.ToDateTime(TimeOnly.MinValue))
            .WithParameter("@to", dateRangeFilter.To.ToDateTime(TimeOnly.MaxValue));

        var queryRequestOptions = new QueryRequestOptions();

        using var feedIterator =
            _container.GetItemQueryIterator<FinancialAccountDto>(queryDefinition, null, queryRequestOptions);

        if (!feedIterator.HasMoreResults) throw new KeyNotFoundException("Account with Owner.Id not found.");

        var result = _taskFactory
            .StartNew(() => feedIterator.ReadNextAsync())
            .Unwrap()
            .GetAwaiter()
            .GetResult()
            .FirstOrDefault();

        return result?.ToFinancialAccount();
    }

    public Transaction[] GetMonthlyTransactions(string accountId, string ownerId,
        GetTransactionsFilters dateRangeFilter)
    {
        const string sqlQuery = $"SELECT t.id, t.amount, t.description, t.category, t.timeStamp FROM c " +
                                $"JOIN t IN c.transactions " +
                                $"WHERE c.id = @idParam " +
                                $"AND c.owner.subId = @ownerId " +
                                $"AND TimestampToDateTime(t.timeStamp) >= @from " +
                                $"AND TimestampToDateTime(t.timeStamp) <= @to " +
                                $"AND (@categories = null " +
                                $"OR ARRAY_CONTAINS(@categories, t.category))";

        var queryDefinition = new QueryDefinition(sqlQuery)
            .WithParameter("@idParam", accountId)
            .WithParameter("@ownerId", ownerId)
            .WithParameter("@to", dateRangeFilter.RangeFilter.To.ToDateTime(TimeOnly.MaxValue))
            .WithParameter("@from", dateRangeFilter.RangeFilter.From.ToDateTime(TimeOnly.MinValue))
            .WithParameter("@categories", dateRangeFilter.Categories?.Select(x => x.Value));

        using var feedIterator =
            _container.GetItemQueryIterator<TransactionDto>(queryDefinition, null, new QueryRequestOptions()
            {
                MaxItemCount = 1
            });

        var transactions = new List<Transaction>();

        while (feedIterator.HasMoreResults)
        {
            var transaction = _taskFactory
                .StartNew(() => feedIterator.ReadNextAsync())
                .Unwrap()
                .GetAwaiter()
                .GetResult()
                .FirstOrDefault();

            if (transaction == null) continue;

            transactions.Add(
                new Transaction(transaction.Id,
                    transaction.Amount, 
                    Description.Create(transaction.Description),
                    Category.Create(transaction.Category), 
                    new TimeStamp(transaction.TimeStamp)));
        }


        return transactions.ToArray();
    }

    public Category[] GetCategoriesByAccount(string accountId, string ownerId)
    {
        const string sqlQuery = $"SELECT DISTINCT VALUE t.category FROM c " +
                                $"JOIN t IN c.transactions " +
                                $"WHERE c.id = @accountId AND c.owner.subId = @ownerId";


        var queryDefinition = new QueryDefinition(sqlQuery)
            .WithParameter("@accountId", accountId)
            .WithParameter("@ownerId", ownerId);
        
        using var feedIterator =
            _container.GetItemQueryIterator<string>(queryDefinition, null, new QueryRequestOptions()
            {
                MaxItemCount = 1
            });

        var categories = new List<Category>();

        while (feedIterator.HasMoreResults)
        {
            var category = _taskFactory
                .StartNew(() => feedIterator.ReadNextAsync())
                .Unwrap()
                .GetAwaiter()
                .GetResult()
                .FirstOrDefault();

            if (category == null) continue;

            categories.Add( Category.Create(category));
        }
        
        return categories.ToArray();
    }
}