namespace Vertr.Terminal.ConsoleApp.Scenarios;

public class SimpleTrading(string hostUrl) : TradingBase(hostUrl)
{
    protected override async Task StartTrading()
    {
        // Bob opens long 
        var t1 = Commands.PlaceOrder(StaticContext.Users.Bob, StaticContext.Symbols.MSFT, 10m, 5);

        // Alice opens short
        var t2 = Commands.PlaceOrder(StaticContext.Users.Alice, StaticContext.Symbols.MSFT, 10m, -5);

        await Task.WhenAll(t1, t2);

        // Bob close long with profit = (15-10)*5 = 25.00
        var t3 = Commands.PlaceOrder(StaticContext.Users.Bob, StaticContext.Symbols.MSFT, 15m, -5);

        // Alice close short with loss = (10-15)*5 = -25.00
        var t4 = Commands.PlaceOrder(StaticContext.Users.Alice, StaticContext.Symbols.MSFT, 15m, 5);

        await Task.WhenAll(t3, t4);
    }
}
