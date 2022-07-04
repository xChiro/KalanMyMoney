using KalanMoney.Domain.UseCases.Adapters;
using KalanMoney.Domain.UseCases.OpenAccount;
using KalanMoney.Domain.UseCases.Tests.RepositoriesMocks;
using Microsoft.Extensions.DependencyInjection;

namespace KalanMoney.Startup;

public static class AccountUseCaseServiceRegistration
{
    public static IServiceCollection AccountUseCaseService(this IServiceCollection services)
    {
        var accountCommandsRepository = new AccountCommandsRepositoryMock();

        services.AddScoped<IOpenAccountInput>(provider => new OpenAccountUseCase(accountCommandsRepository));
        services.AddSingleton(accountCommandsRepository);

        return services;
    }
}