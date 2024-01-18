using Vertr.Terminal.ConsoleApp.Scenarios;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    private const string _terminalHostUrl = "http://localhost:5010";

    public static async Task Main()
    {
        var trading = new RandomWalkTrading(
            _terminalHostUrl,
            basePrice: 10.45m,
            numberOfIterations: 10);

        await trading.Execute();
    }
}
