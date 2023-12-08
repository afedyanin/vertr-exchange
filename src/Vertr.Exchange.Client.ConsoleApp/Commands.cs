using Vert.Exchange.Client.Host.ApiClient;
using Vertr.Exchange.Client.ConsoleApp.StaticData;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Client.ConsoleApp;
internal static class Commands
{
    public static async Task<ApiCommandResult?> AddSymbols(IHostApiClient client)
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = Symbols.All.Select(s => s.GetSpecification()).ToArray(),
        };

        var res = await client.AddSymbols(req);
        return res;
    }

    public static async Task<ApiCommandResult?> Reset(IHostApiClient client)
    {
        var res = await client.Reset();
        return res;
    }

    public static async Task<ApiCommandResult?> Nop(IHostApiClient client)
    {
        var res = await client.Nop();
        return res;
    }

    public static async Task<ApiCommandResult?> AddUsers(IHostApiClient client)
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts =
            [
                UserAccounts.AliceAccount.ToDto(),
                UserAccounts.BobAccount.ToDto()
            ],
        };

        var res = await client.AddAccounts(req);
        return res;
    }

    public static async Task<ApiCommandResult?> PlaceOrder(
        IHostApiClient client,
        User user,
        Symbol symbol,
        decimal price,
        long size)
    {
        var req = CreatePlaceOrderRequest(user, symbol, price, size);
        var res = await client.PlaceOrder(req);
        return res;
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
