using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GamerZone.UserManagerService.Api.Startup))]

public class FunctionsStartupAttribute : Attribute
{
    public FunctionsStartupAttribute(Type type)
    {
        throw new NotImplementedException();
    }
}

namespace GamerZone.UserManagerService.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // var appSetting = new AppSetting()
            // {
            //     ClientID = GetEnvironmentVariable("clientID"),
            //     TenantID = GetEnvironmentVariable("tenantID"),
            //     VaultURI = GetEnvironmentVariable("VaultUri"),
            //     SecretClient = GetEnvironmentVariable("secretClient"),
            //     EndpointUri = GetEnvironmentVariable("dbEndpointUri"),
            //     PrimaryKey = GetEnvironmentVariable("dbPrimaryKey"),
            //     DataBaseId = GetEnvironmentVariable("dataBaseId"),
            //     UserContainerId = GetEnvironmentVariable("userContainerId")
            // };
            //
            // builder.Services.AddApplicationServices();
            // builder.Services.AddPersistenceServices(appSetting);
            // builder.Services.AddInfrastructureServices(appSetting);
            //
            // builder.Services.AddSingleton(appSetting);
        }

        private static string GetEnvironmentVariable(string name) 
        {
            return System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);
        }
    }
}