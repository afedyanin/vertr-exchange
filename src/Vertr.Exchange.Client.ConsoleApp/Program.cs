using Refit;
using Vert.Exchange.Client.Host.ApiClient;
using Vertr.Exchange.Client.ConsoleApp.StaticData;
using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;
namespace Vertr.Exchange.Client.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var exchApi = RestService.For<IHostApiClient>("http://localhost:5010");
        var res = await exchApi.Reset();
        Console.WriteLine(res);
    }

    private static AddSymbolsRequest CreateAddSymbolsRequest()
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = Symbols.All.Select(s => s.GetSpecification()).ToArray(),
        };
        return req;
    }

    private static AddAccountsRequest CreateAddAccountsRequest()
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts =
            [
                UserAccounts.AliceAccount.ToDto(),
                UserAccounts.BobAccount.ToDto()
            ],
        };

        return req;
    }

    private static PlaceOrderRequest CreatePlaceOrderRequest(
        User user,
        Symbol symbol,
        decimal price,
        long size)
    {
        var req = new PlaceOrderRequest()
        {
            UserId = user.Id,
            Symbol = symbol.Id,
            Price = price,
            Size = Math.Abs(size),
            Action = size > 0 ? OrderAction.BID : OrderAction.ASK,
            OrderType = price == decimal.Zero ? OrderType.IOC : OrderType.GTC,
        };

        return req;
    }
}
