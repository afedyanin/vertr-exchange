using Refit;
using Vert.Exchange.Client.Host.ApiClient;
using Vertr.Exchange.Client.ConsoleApp.StaticData;
namespace Vertr.Exchange.Client.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var exchApi = RestService.For<IHostApiClient>("http://localhost:5010");

        var res = await Commands.Reset(exchApi);
        Console.WriteLine(res);

        res = await Commands.AddSymbols(exchApi);
        Console.WriteLine(res);

        res = await Commands.AddUsers(exchApi);
        Console.WriteLine(res);

        res = await Commands.Nop(exchApi);
        Console.WriteLine(res);

        res = await Commands.PlaceOrder(exchApi, Users.Bob, Symbols.MSFT, 100m, 5);
        Console.WriteLine(res);
    }
}
