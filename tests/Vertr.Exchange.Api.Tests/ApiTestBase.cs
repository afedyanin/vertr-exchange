using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Commands.Queries;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Messages;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Shared.Reports;

namespace Vertr.Exchange.Api.Tests;
public abstract class ApiTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected IExchangeApi Api { get; private set; }

    protected MessageHandlerStub MessageHandler { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        ServiceProvider = ServiceProviderStub.BuildServiceProvider();
    }

    [SetUp]
    public void Setup()
    {
        Api = ServiceProvider.GetRequiredService<IExchangeApi>();
        MessageHandler = ServiceProvider.GetRequiredService<MessageHandlerStub>();
    }

    [TearDown]
    public void TearDown()
    {
        Api?.Dispose();
    }

    protected async Task<Common.Messages.ApiCommandResult> SendAsync(ApiCommandBase cmd)
    {
        Api.Send(cmd);
        await Task.Delay(1);
        var res = MessageHandler.GetApiCommandResult(cmd.OrderId);

        Assert.That(res, Is.Not.Null);
        return res;
    }

    protected TradeEvent GetTradeEvent(long takerOrderId)
    {
        var res = MessageHandler.GetTradeEvent(takerOrderId);
        Assert.That(res, Is.Not.Null);
        return res;
    }

    protected ReduceEvent GetReduceEvent(long orderId)
    {
        var res = MessageHandler.GetReduceEvent(orderId);
        Assert.That(res, Is.Not.Null);
        return res;
    }

    protected RejectEvent GetRejectEvent(long orderId)
    {
        var res = MessageHandler.GetRejectEvent(orderId);
        Assert.That(res, Is.Not.Null);
        return res;
    }

    protected async Task AddUser(long userId)
    {
        var orderId = 1000L;
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

        var cmd = new AddSymbolsCommand(1010L, DateTime.UtcNow, [symSpec]);
        var res = await SendAsync(cmd);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    protected async Task<SingleUserReportResult?> GetUserReport(long uid)
    {
        var rep = new SingleUserReport(1020L, DateTime.UtcNow, uid);
        var res = await SendAsync(rep);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        if (res == null ||
            res.ResultCode != CommandResultCode.SUCCESS ||
            res.BinaryCommandType != BinaryDataType.QUERY_SINGLE_USER_REPORT)
        {
            return null;
        }

        var report = JsonSerializer.Deserialize<SingleUserReportResult>(res.BinaryData);
        return report;
    }

    protected async Task<OrderBook?> GetOrderBook(int symbol)
    {
        var obr = new OrderBookRequest(23789L, DateTime.UtcNow, symbol, 100);
        var res = await SendAsync(obr);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        if (res == null ||
            res.ResultCode != CommandResultCode.SUCCESS ||
            res.BinaryCommandType != BinaryDataType.QUERY_SINGLE_USER_REPORT)
        {
            return null;
        }

        var book = JsonSerializer.Deserialize<OrderBook>(res.BinaryData);
        return book;
    }

    protected async Task<Common.Messages.ApiCommandResult> PlaceGTCOrder(
        OrderAction orderAction,
        long uid,
        int symbol,
        decimal price,
        long size,
        long orderId)
    {
        var cmd = new PlaceOrderCommand(
            orderId,
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
