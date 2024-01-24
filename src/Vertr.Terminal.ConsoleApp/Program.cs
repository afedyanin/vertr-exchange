using Vertr.Terminal.ConsoleApp.Scenarios;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    private const string _terminalHostUrl = "http://localhost:5010";

    public static async Task Main()
    {
        // var trading = new OverlappedOrdersTrading(_terminalHostUrl);

        var trading = new RandomWalkTrading(
            _terminalHostUrl,
            basePrice: 100m,
            priceDelta: 0.9m,
            numberOfIterations: 100);

        await trading.Execute();
    }
}
