using Vertr.Terminal.ApiClient.Contracts;
using static Vertr.Terminal.ConsoleApp.StaticContext;

namespace Vertr.Terminal.ConsoleApp.Scenarios;
public class RandomWalkTrading(
    string terminalHostUrl,
    decimal basePrice,
    decimal priceDelta,
    int numberOfIterations) : TradingBase(terminalHostUrl, Symbols.MSFT)
{

    private readonly decimal _basePrice = basePrice;
    private readonly decimal _priceDelta = priceDelta;
    private readonly int _numberOfIterations = numberOfIterations;

    protected override async Task StartTrading()
    {
        var t1 = Task.Run(async () =>
        {
            await Commands.RandomWalk(Users.Bob, Symbol, _basePrice, _priceDelta, _numberOfIterations);
        });

        var t2 = Task.Run(async () =>
        {
            await Commands.RandomWalk(Users.Alice, Symbol, _basePrice, _priceDelta, _numberOfIterations);
        });

        await Task.WhenAll(t1, t2);

        await DumpResults();
    }

    protected async Task TryToClosePosition(User user)
    {
        var report = await Commands.GetSingleUserReport(user);

        if (report == null)
        {
            return;
        }

        if (!report.Positions.TryGetValue(Symbol.Id, out var position))
        {
            return;
        }

        if (position.Direction == Exchange.Shared.Enums.PositionDirection.EMPTY)
        {
            return;
        }

        if (position.OpenVolume == decimal.Zero)
        {
            return;
        }

        var reverseSign = position.Direction == Exchange.Shared.Enums.PositionDirection.DIR_SHORT ? 1 : -1;
        var res = await Commands.PlaceOrder(user, Symbol, decimal.Zero, (long)position.OpenVolume * reverseSign);

        if (res == null)
        {
            Console.WriteLine($"ERROR: Failed to close position for user={user.Name}");
            return;
        }

        Console.WriteLine($"Close position: User={user.Name} OrderId={res.OrderId} Code={res.ResultCode}");
    }
}
