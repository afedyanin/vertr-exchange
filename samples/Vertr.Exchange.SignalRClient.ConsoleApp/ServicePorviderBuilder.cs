using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Vertr.Exchange.SignalRClient.ConsoleApp;
internal static class ServicePorviderBuilder
{
    public static IServiceProvider BuildServiceProvider(IConfiguration configuration)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeApi(configuration);
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        serviceCollection.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}
