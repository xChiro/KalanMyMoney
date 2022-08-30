using System.Net;
using KalanMoney.Domain.Entities;
using KalanMoney.Domain.Entities.ValueObjects;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Persistence.CosmosDB.DTOs;
using Microsoft.Azure.Cosmos;

namespace KalanMoney.Persistence.CosmosDB.Repositories;

public class AccountCommandsRepository : IAccountCommandsRepository
{
    private readonly Container _container;
    private readonly TaskFactory _taskFactory;

    public AccountCommandsRepository(Container container)
    {
        _container = container;
        _taskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);
    }

    public void OpenAccount(FinancialAccount account)
    {
        var accountDto = FinancialAccountDto.FromFinancialAccount(account);
        var resultTask = _container.CreateItemAsync(accountDto);

        var result = _taskFactory
            .StartNew(() => resultTask)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

        if (result.StatusCode != HttpStatusCode.Created) throw new Exception();
    }

    public void StoreTransaction(string accountId, Balance accountBalance, Transaction transaction)
    {
        var transactionDto = new TransactionDto(transaction.Id, transaction.Amount, transaction.Description.Value,
            transaction.Category.Value, transaction.TimeStamp.Value);

        var result = _taskFactory.StartNew(() =>
                _container.PatchItemAsync<FinancialAccountDto>(accountId, new PartitionKey(accountId),
                    new[]
                    {
                        PatchOperation.Add("Transactions/1", transactionDto),
                        PatchOperation.Replace("Balance", accountBalance),
                    }))
            .Unwrap()
            .GetAwaiter()
            .GetResult();

        if (result.StatusCode != HttpStatusCode.Created) throw new Exception();
    }
}