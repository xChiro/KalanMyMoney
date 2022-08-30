using System.Text.Json;
using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Repositories;
using KalanMoney.Persistence.CosmosDB.Repositories;
using KalanMoney.Persistence.MemoryDatabase;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace KalanMoney.Startup;

public static class AccountUseCaseServiceRegistration
{
    public static IServiceCollection SetupWithMemoryDataBase(this IServiceCollection services)
    {
        var memoryDb = new MemoryDb();
        var accountMemoryRepository = new AccountsMemoryRepository(memoryDb);
        
        services.AddScoped<IOpenAccountInput>(_ => new OpenAccountUseCase(accountMemoryRepository));
        services.AddScoped<IAddIncomeTransactionInput>(_ => new AddIncomeTransactionUseCase(accountMemoryRepository, accountMemoryRepository));
        services.AddScoped<IAddOutcomeTransactionInput>(_ => new AddOutcomeTransactionUseCase(accountMemoryRepository, accountMemoryRepository));
        services.AddScoped<IAccountDashboardInput>(_ => new AccountDashboardUseCase(accountMemoryRepository));
        
        services.AddSingleton(accountMemoryRepository);
        return services;
    }
    
    public static IServiceCollection SetupWithCosmosDataBase(this IServiceCollection services)
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
        var container = client.GetDatabase(dataBaseId).GetContainer(accountContainerId);
        var accountQueriesRepository = new AccountQueriesRepository(container);
        var accountCommandsRepository = new AccountCommandsRepository(container);
            
        services.AddScoped<IOpenAccountInput>(_ => new OpenAccountUseCase(accountCommandsRepository));
        services.AddScoped<IAddIncomeTransactionInput>(_ => new AddIncomeTransactionUseCase(accountQueriesRepository, accountCommandsRepository));
        services.AddScoped<IAddOutcomeTransactionInput>(_ => new AddOutcomeTransactionUseCase(accountQueriesRepository, accountCommandsRepository));
        services.AddScoped<IAccountDashboardInput>(_ => new AccountDashboardUseCase(accountQueriesRepository));

        return services;
    }
    
    private static string? GetEnvironmentVariable(string name) 
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}