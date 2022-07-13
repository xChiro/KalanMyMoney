using KalanMoney.Domain.UseCases.Adapters;
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
        var accountCommandsRepository = new AccountsMemoryRepository(memoryDb);
        var categoryCommandsRepository = new CategoryMemoryRepository(memoryDb);

        services.AddScoped<IOpenAccountInput>(_ => new OpenAccountUseCase(accountCommandsRepository));
        services.AddScoped<ICreateCategoryInput>(_ => new CreateCategoryUseCase(categoryCommandsRepository, accountCommandsRepository));
        
        services.AddSingleton(accountCommandsRepository);
        services.AddSingleton(categoryCommandsRepository);

        return services;
    }
}