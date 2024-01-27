using Vertr.Exchange.Domain.RiskEngine;
using Vertr.Exchange.Domain.Accounts;
using Vertr.Exchange.Domain.MatchingEngine;
using Vertr.Exchange.Application;
using Vertr.Exchange.Application.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Vertr.Exchange.ConsoleApp;
internal static class ServicePorviderBuilder
{
    public static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeCommandsApi();
        serviceCollection.AddAccounts();
        serviceCollection.AddRiskEngine();
        serviceCollection.AddMatchingEngine();

        serviceCollection.AddSingleton<LogMessageHandler>();
        serviceCollection.AddSingleton<IMessageHandler>(
                    x => x.GetRequiredService<LogMessageHandler>());

        serviceCollection.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}
