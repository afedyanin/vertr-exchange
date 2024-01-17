

namespace Vertr.Terminal.ConsoleApp.Scenarios;
public class RandomWalkTrading(string hostUrl) : TradingBase(hostUrl)
{
    protected override async Task StartTrading()
    {
        var bobTrading = Task.Run(async () =>
        {
            await Commands.RandomWalk(StaticContext.Users.Bob, StaticContext.Symbols.MSFT, 100, 10);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await Commands.RandomWalk(StaticContext.Users.Alice, StaticContext.Symbols.MSFT, 100, 10);
        });

        await Task.WhenAll(aliceTrading, bobTrading);
    }
}
