using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.AddIncomeTransaction;
using KalanMoney.Domain.UseCases.CreateCategory;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Persistence.MemoryDatabase;
using Microsoft.Extensions.DependencyInjection;

namespace KalanMoney.Startup;

public static class AccountUseCaseServiceRegistration
{
    public static IServiceCollection AccountUseCaseService(this IServiceCollection services)
    {
        var accountCommandsRepository = new AccountsMemoryRepository();

        services.AddScoped<IOpenAccountInput>(provider => new OpenAccountUseCase(accountCommandsRepository));
        services.AddSingleton(accountCommandsRepository);

        return services;
    }
}