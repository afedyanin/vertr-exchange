using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ServiceProviderStub
{
    public static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeApi();

        serviceCollection.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}
