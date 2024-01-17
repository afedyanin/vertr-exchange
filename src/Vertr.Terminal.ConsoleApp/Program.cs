using Vertr.Terminal.ConsoleApp.Scenarios;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    private const string _hostUrl = "http://localhost:5010";

    public static async Task Main()
    {
        var trading = new SimpleTrading(_hostUrl);
        // var trading = new RandomWalkTrading(_hostUrl);
        await trading.Execute();
    }
}
