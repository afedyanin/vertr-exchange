using static Vertr.Terminal.ConsoleApp.StaticContext;
namespace Vertr.Terminal.ConsoleApp.Scenarios;

public class OverlappedOrdersTrading(string terminalHostUrl) : TradingBase(terminalHostUrl, Symbols.MSFT)
{
    protected override async Task StartTrading()
    {
        // stage 1 - open positions
        var t1 = PlaceAsk(Users.Alice, 10, 15);
        var t2 = PlaceBid(Users.Bob, 10, 5);
        await Task.WhenAll(t1, t2);
        //await DumpResults();

        // stage 2 - close positions
        var t3 = PlaceAsk(Users.Bob, 10, 5 + 10);
        var t4 = PlaceBid(Users.Alice, 10, 15 + 10);
        await Task.WhenAll(t3, t4);
        //await DumpResults();

        // stage 3 - close positions
        var t5 = PlaceBid(Users.Bob, 10, 10);
        var t6 = PlaceAsk(Users.Alice, 10, 10);
        await Task.WhenAll(t5, t6);
        await DumpResults();
    }
}

