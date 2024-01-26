using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Application.Generators;
using Vertr.Exchange.Domain.RiskEngine;
using Vertr.Exchange.Domain.Accounts;
using Vertr.Exchange.Domain.MatchingEngine;
using Vertr.Exchange.Application.Messages;

namespace Vertr.Exchange.Application.Tests.Stubs;

internal static class ServiceProviderStub
{
    public static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeCommandsApi();
        serviceCollection.AddAccounts();
        serviceCollection.AddRiskEngine();
        serviceCollection.AddMatchingEngine();

        serviceCollection.AddSingleton<MessageHandlerStub>();
        serviceCollection.AddSingleton<IMessageHandler>(
                    x => x.GetRequiredService<MessageHandlerStub>());
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();

        serviceCollection.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}
