using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Application.Tests.Stubs;
using Vertr.Exchange.Application.Generators;
using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Application.Commands.Queries;
using Vertr.Exchange.Application.Messages;
using Vertr.Exchange.Application.Commands.Api;
using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Reports;

namespace Vertr.Exchange.Application.Tests;
public abstract class ApiTestBase
{
    private const int _cancellationTimeout = 100;

    protected IServiceProvider ServiceProvider { get; private set; }

    protected IOrderIdGenerator OrderIdGenerator { get; private set; }

    protected IExchangeCommandsApi Api { get; private set; }

    protected MessageHandlerStub MessageHandler { get; private set; }

    [SetUp]
    public void Setup()
    {
        ServiceProvider = ServiceProviderStub.BuildServiceProvider();
        MessageHandler = ServiceProvider.GetRequiredService<MessageHandlerStub>();
        OrderIdGenerator = ServiceProvider.GetRequiredService<IOrderIdGenerator>();
        Api = ServiceProvider.GetRequiredService<IExchangeCommandsApi>();
    }

    [TearDown]
    public void TearDown()
    {
        Api?.Dispose();
    }

    protected async Task<ApiCommandResult> SendAsync(ApiCommandBase cmd)
    {
        Api.Send(cmd);
        var cts = new CancellationTokenSource(_cancellationTimeout);
        var res = await MessageHandler.GetApiCommandResult(cmd.OrderId, cts.Token);
        return res;
    }

    protected async Task<TradeEvent> GetTradeEvent(long takerOrderId)
    {
        var cts = new CancellationTokenSource(_cancellationTimeout);
        var res = await MessageHandler.GetTradeEvent(takerOrderId, cts.Token);
        return res;
    }

    protected async Task<ReduceEvent> GetReduceEvent(long orderId)
    {
        var cts = new CancellationTokenSource(_cancellationTimeout);
        var res = await MessageHandler.GetReduceEvent(orderId, cts.Token);
        return res;
    }

    protected async Task<RejectEvent> GetRejectEvent(long orderId)
    {
        var cts = new CancellationTokenSource(_cancellationTimeout);
        var res = await MessageHandler.GetRejectEvent(orderId, cts.Token);
        return res;
    }

    protected async Task AddUser(long userId)
    {
        var orderId = OrderIdGenerator.NextId;
        var cmd = new AddUserCommand(orderId, DateTime.UtcNow, userId);
        var res = await SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    protected async Task AddSymbol(
        int symbol,
        int currency = 100,
        SymbolType symbolType = SymbolType.CURRENCY_EXCHANGE_PAIR)
    {
        var symSpec = new SymbolSpecification
        {
            Currency = currency,
            Type = symbolType,
            SymbolId = symbol
        };

        var cmd = new AddSymbolsCommand(OrderIdGenerator.NextId, DateTime.UtcNow, [symSpec]);
        var res = await SendAsync(cmd);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    protected async Task<SingleUserReportResult?> GetUserReport(long uid)
    {
        var rep = new SingleUserReport(OrderIdGenerator.NextId, DateTime.UtcNow, uid);
        var res = await SendAsync(rep);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        if (res.BinaryCommandType != BinaryDataType.QUERY_SINGLE_USER_REPORT)
        {
            return null;
        }

        var report = JsonSerializer.Deserialize<SingleUserReportResult>(res.BinaryData);
        return report;
    }

    protected async Task<OrderBook?> GetOrderBook(int symbol)
    {
        var obr = new OrderBookRequest(OrderIdGenerator.NextId, DateTime.UtcNow, symbol, 100);
        var res = await SendAsync(obr);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var cts = new CancellationTokenSource(_cancellationTimeout);
        var book = await MessageHandler.GetOrderBook(symbol, cts.Token);
        return book;
    }

    protected async Task<ApiCommandResult> PlaceGTCOrder(
        OrderAction orderAction,
        long uid,
        int symbol,
        decimal price,
        long size)
    {
        var cmd = new PlaceOrderCommand(
            OrderIdGenerator.NextId,
            DateTime.UtcNow,
            price,
            size,
            orderAction,
            OrderType.GTC,
            uid,
            symbol);

        var res = await SendAsync(cmd);
        return res;
    }
}
