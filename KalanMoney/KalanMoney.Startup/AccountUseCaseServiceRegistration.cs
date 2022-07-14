using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.AddOutcomeTransaction;
using KalanMoney.Domain.UseCases.CreateCategory;
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
        var categoryMemoryRepository = new CategoryMemoryRepository(memoryDb);

        services.AddScoped<IOpenAccountInput>(_ => new OpenAccountUseCase(accountMemoryRepository));
        services.AddScoped<ICreateCategoryInput>(_ => new CreateCategoryUseCase(categoryMemoryRepository, accountMemoryRepository));
        services.AddScoped<IAddIncomeTransactionInput>(_ => new AddIncomeTransactionUseCase(accountMemoryRepository, categoryMemoryRepository, accountMemoryRepository));
        services.AddScoped<IAddOutcomeTransactionInput>(_ => new AddOutcomeTransactionUseCase(accountMemoryRepository, categoryMemoryRepository, accountMemoryRepository));
        
        services.AddSingleton(accountMemoryRepository);
        services.AddSingleton(categoryMemoryRepository);

        return services;
    }
}