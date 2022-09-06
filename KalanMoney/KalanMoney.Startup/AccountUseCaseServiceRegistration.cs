using System.Text.Json;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.GetMonthlyTransactions;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Persistence.CosmosDB.Repositories;
using KalanMoney.Persistence.MemoryDatabase;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace KalanMoney.Startup;

public static class AccountUseCaseServiceRegistration
{
    public static IServiceCollection SetupFromSettings(this IServiceCollection services, string strategy)
    {
        return strategy.ToLower() switch
        {
            "cosmosdb" => SetupWithCosmosDataBase(services),
            _ => SetupWithMemoryDataBase(services)
        };
    }
    
    public static IServiceCollection SetupWithMemoryDataBase(this IServiceCollection services)
    {
        var memoryDb = new MemoryDb();
        var accountMemoryRepository = new AccountsMemoryRepository(memoryDb);
        
        InjectDependencies(services, accountMemoryRepository, accountMemoryRepository);
        
        services.AddSingleton(accountMemoryRepository);
        return services;
    }
    
    public static IServiceCollection SetupWithCosmosDataBase(this IServiceCollection services)
    {
        var container = SetupCosmosDbContainer();
        var accountQueriesRepository = new AccountQueriesRepository(container);
        var accountCommandsRepository = new AccountCommandsRepository(container);

        InjectDependencies(services, accountCommandsRepository, accountQueriesRepository);

        return services;
    }

    private static Container SetupCosmosDbContainer()
    {
        var endpoint = GetEnvironmentVariable("dbEndpointUri");
        var primaryKey = GetEnvironmentVariable("dbPrimaryKey");
        var dataBaseId = GetEnvironmentVariable("dataBaseId");
        var accountContainerId = GetEnvironmentVariable("accountsContainerId");

        var serializerOptions = new CustomCosmosSerializer(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
            
        var client = new CosmosClientBuilder(endpoint, primaryKey).WithCustomSerializer(serializerOptions).Build();
        
        return client.GetDatabase(dataBaseId).GetContainer(accountContainerId);
    }

    private static void InjectDependencies(IServiceCollection services, IAccountCommandsRepository accountCommandsRepository, 
        IAccountQueriesRepository accountQueriesRepository)
    {
        services.AddScoped<IOpenAccountInput>(_ => new OpenAccountUseCase(accountCommandsRepository));
        services.AddScoped<IAddIncomeTransactionInput>(_ => new AddIncomeTransactionUseCase(accountQueriesRepository, accountCommandsRepository));
        services.AddScoped<IAddOutcomeTransactionInput>(_ => new AddOutcomeTransactionUseCase(accountQueriesRepository, accountCommandsRepository));
        services.AddScoped<IAccountDashboardInput>(_ => new AccountDashboardUseCase(accountQueriesRepository));
        services.AddScoped<IGetMonthlyTransactionsInput>(_ => new GetMonthlyTransactionsUseCase(accountQueriesRepository));
    }
    
    private static string? GetEnvironmentVariable(string name) 
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}