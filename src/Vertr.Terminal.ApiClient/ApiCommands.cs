using System.Text.Json;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Shared.Reports;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ApiClient;
public sealed class ApiCommands(ITerminalApiClient client)
{
    private readonly ITerminalApiClient _client = client;
    public async Task Reset()
    {
        var res = await _client.Reset();
        EnsureSuccess(res);
    }

    public async Task AddSymbols(Symbol[] symbols)
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = symbols.Select(s => s.GetSpecification()).ToArray(),
        };

        var res = await _client.AddSymbols(req);
        EnsureSuccess(res);
    }
    public async Task AddUsers(Contracts.UserAccount[] userAccounts)
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts = userAccounts.Select(ua => ua.ToDto()).ToArray(),
        };

        var res = await _client.AddAccounts(req);
        EnsureSuccess(res);
    }

    public async Task Nop()
    {
        var res = await _client.Nop();
        EnsureSuccess(res);
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

    public async Task<OrderBook[]> GetOrderBooks()
    {
        var res = await _client.GetOrderBooks();
        return res;
    }

    public async Task<TradeEvent[]> GetTrades()
    {
        var res = await _client.GetTrades();
        return res;
    }

    public async Task<OrderDto[]> GetOrders()
    {
        var res = await _client.GetOrders();
        return res;
    }

    public async Task<PortfolioDto[]> GetPortfolios()
    {
        var res = await _client.GetPortfolios();
        return res;
    }

    public async Task<MarketDataItemDto[]> GetMarketDataSnapshot()
    {
        var res = await _client.GetMarketDataSnapshot();
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

    public async Task<SingleUserReportResult?> GetSingleUserReport(User user)
    {
        var request = new UserRequest
        {
            UserId = user.Id,
        };

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

    public void EnsureSuccess(ApiCommandResult? result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.ResultCode != CommandResultCode.SUCCESS)
        {
            throw new InvalidOperationException($"Invalid result code: {result.ResultCode}");
        }
    }
}
