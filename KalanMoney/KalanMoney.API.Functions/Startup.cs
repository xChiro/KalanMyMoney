using System;
using KalanMoney.API.Functions;
using KalanMoney.Startup;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace KalanMoney.API.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {            
            var strategy = Environment.GetEnvironmentVariable("deployStrategy", EnvironmentVariableTarget.Process);

            builder.Services.SetupFromSettings(strategy ?? "");
        }
    }
}