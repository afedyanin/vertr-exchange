using static Vertr.Terminal.ConsoleApp.StaticContext;

namespace Vertr.Terminal.ConsoleApp.Scenarios;

public class SimpleTrading(string terminalHostUrl) : TradingBase(terminalHostUrl, Symbols.MSFT)
{
    protected override async Task StartTrading()
    {
        // stage 1 - open positions
        var t1 = PlaceAsk(Users.Alice, 10, 5);
        var t2 = PlaceBid(Users.Bob, 10, 5);
        await Task.WhenAll(t1, t2);
        await DumpResults();

        // stage 2 - close positions
        var t3 = PlaceAsk(Users.Bob, 15, 5);
        var t4 = PlaceBid(Users.Alice, 15, 5);
        await Task.WhenAll(t3, t4);
        await DumpResults();
    }
}
