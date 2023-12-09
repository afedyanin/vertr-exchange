using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp;
internal sealed class Commands(ITerminalApiClient client)
{
    private readonly ITerminalApiClient _client = client;

    public async Task<ApiCommandResult?> AddSymbols()
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = Symbols.All.Select(s => s.GetSpecification()).ToArray(),
        };

        var res = await _client.AddSymbols(req);
        return res;
    }

    public async Task<ApiCommandResult?> Reset()
    {
        var res = await _client.Reset();
        return res;
    }

    public async Task<ApiCommandResult?> Nop()
    {
        var res = await _client.Nop();
        return res;
    }

    public async Task<ApiCommandResult?> AddUsers()
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts =
            [
                UserAccounts.AliceAccount.ToDto(),
                UserAccounts.BobAccount.ToDto()
            ],
        };

        var res = await _client.AddAccounts(req);
        return res;
    }

    public async Task<ApiCommandResult?> PlaceOrder(
        User user,
        Symbol symbol,
        decimal price,
        long size)
    {
        var req = CreatePlaceOrderRequest(user, symbol, price, size);
        var res = await _client.PlaceOrder(req);
        return res;
    }

    public async Task<ApiCommandResult?> GetSingleUserReport(UserRequest request)
    {
        var res = await _client.GetSingleUserReport(request);
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
