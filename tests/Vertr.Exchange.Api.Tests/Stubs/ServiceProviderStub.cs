using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.RiskEngine;
using Vertr.Exchange.Accounts;
using Vertr.Exchange.MatchingEngine;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Core;
using Vertr.Exchange.Api.Generators;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ServiceProviderStub
{
    public static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddExchangeCore();
        serviceCollection.AddExchangeApi();
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
