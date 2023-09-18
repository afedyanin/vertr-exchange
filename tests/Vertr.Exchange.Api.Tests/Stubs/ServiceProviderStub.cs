using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Infrastructure;
using Vertr.Exchange.RiskEngine;
using Vertr.Exchange.Accounts;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ServiceProviderStub
{
    public static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeApi();
        serviceCollection.AddUserProfileProvider();
        serviceCollection.AddOrderRiskEngine();
        serviceCollection.UseRiskEngine();
        // serviceCollection.UseMatchingEngine();

        serviceCollection.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}
