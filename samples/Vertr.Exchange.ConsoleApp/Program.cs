using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Application.Generators;
using Vertr.Exchange.Domain.RiskEngine;
using Vertr.Exchange.Domain.Accounts;
using Vertr.Exchange.Domain.MatchingEngine;
using Vertr.Exchange.Application;
using Vertr.Exchange.Application.Messages;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Application.Commands.Api;
using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Application.Commands;
using static Vertr.Exchange.ConsoleApp.StaticContext;

namespace Vertr.Exchange.ConsoleApp;

internal static class Program
{
    public static async Task Main()
    {
        var sp = BuildServiceProvider();
        var idGen = sp.GetRequiredService<IOrderIdGenerator>();
        var api = sp.GetRequiredService<IExchangeCommandsApi>();

        api.Send(new AddSymbolsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            Symbols.AllSymbolSpecs));

        api.Send(new AddAccountsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            users: UserAccounts.All));

        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 120.45m,
            size: 10,
            action: OrderAction.ASK,
            orderType: OrderType.GTC,
            uid: Users.Alice.Id,
            symbol: Symbols.MSFT.Id));

        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 123.56m,
            size: 7,
            action: OrderAction.BID,
            orderType: OrderType.GTC,
            uid: Users.Bob.Id,
            symbol: Symbols.MSFT.Id));

        await Task.Delay(2000);
    }

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
