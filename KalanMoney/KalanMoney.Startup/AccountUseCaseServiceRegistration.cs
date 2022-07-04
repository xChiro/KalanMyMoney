using Microsoft.Extensions.DependencyInjection;

namespace KalanMoney.Startup;

public static class AccountUseCaseServiceRegistration
{
    public static IServiceCollection Setup(this IServiceCollection services)
    {
        throw new NotImplementedException();
        // services.AddScoped<IOpenAccountInput>(provider => new OpenAccountUseCase());
    }
}