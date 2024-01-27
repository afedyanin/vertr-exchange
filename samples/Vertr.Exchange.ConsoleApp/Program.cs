using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Application.Generators;
using Vertr.Exchange.Application.Commands.Api;
using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Application.Commands;
using static Vertr.Exchange.ConsoleApp.StaticContext;

namespace Vertr.Exchange.ConsoleApp;

internal static class Program
{
    public static async Task Main()
    {
        var sp = ServicePorviderBuilder.BuildServiceProvider();
        var idGen = sp.GetRequiredService<IOrderIdGenerator>();
        var api = sp.GetRequiredService<IExchangeCommandsApi>();

        // Add Symbols
        api.Send(new AddSymbolsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            Symbols.AllSymbolSpecs));

        // Add Users and Accounts
        api.Send(new AddAccountsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            users: UserAccounts.All));

        // Place ASK order
        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 120.45m,
            size: 10,
            action: OrderAction.ASK,
            orderType: OrderType.GTC,
            uid: Users.Alice.Id,
            symbol: Symbols.MSFT.Id));

        // Place BID order
        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 123.56m,
            size: 7,
            action: OrderAction.BID,
            orderType: OrderType.GTC,
            uid: Users.Bob.Id,
            symbol: Symbols.MSFT.Id));

        // wait to end processing
        await Task.Delay(2000);
    }
}
