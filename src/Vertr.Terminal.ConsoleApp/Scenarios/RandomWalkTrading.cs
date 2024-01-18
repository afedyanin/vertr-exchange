using static Vertr.Terminal.ConsoleApp.StaticContext;

namespace Vertr.Terminal.ConsoleApp.Scenarios;
public class RandomWalkTrading(
    string hostUrl,
    decimal basePrice,
    int numberOfIterations) : TradingBase(hostUrl, Symbols.MSFT)
{

    private readonly decimal _basePrice = basePrice;
    private readonly int _numberOfIterations = numberOfIterations;

    protected override async Task StartTrading()
    {
        var t1 = Task.Run(async () =>
        {
            await Commands.RandomWalk(Users.Bob, Symbol, _basePrice, _numberOfIterations);
        });

        var t2 = Task.Run(async () =>
        {
            await Commands.RandomWalk(Users.Alice, Symbol, _basePrice, _numberOfIterations);
        });

        await Task.WhenAll(t1, t2);
        await DumpResults();
    }
}
