using Vertr.Exchange.Contracts;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient;
using System.Text.Json;
using Vertr.Exchange.Shared.Reports;
using Vertr.Terminal.ApiClient.Contracts;


namespace Vertr.Terminal.ConsoleApp;
internal sealed class Commands(ITerminalApiClient client)
{
    private readonly ITerminalApiClient _client = client;

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

    public async Task RandomWalk(
        User user,
        Symbol symbol,
        decimal startPrice,
        int ordersCount)
    {
        var req = new RandomWalkRequest()
        {
            UserId = user.Id,
            SymbolId = symbol.Id,
            BasePrice = startPrice,
            PriceDelta = 0.01m,
            OrdersCount = ordersCount
        };

        await _client.RandomWalk(req);
    }

    public async Task<SingleUserReportResult?> GetSingleUserReport(UserRequest request)
    {
        var res = await _client.GetSingleUserReport(request);

        if (res == null ||
            res.ResultCode != CommandResultCode.SUCCESS ||
            res.BinaryCommandType != BinaryDataType.QUERY_SINGLE_USER_REPORT)
        {
            return null;
        }

        var report = JsonSerializer.Deserialize<SingleUserReportResult>(res.BinaryData);
        return report;
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
