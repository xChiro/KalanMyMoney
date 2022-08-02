using KalanMoney.Domain.UseCases.AccountDashboard;
using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Persistence.MemoryDatabase;
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
}