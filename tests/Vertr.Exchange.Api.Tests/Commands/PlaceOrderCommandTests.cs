using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class PlaceOrderCommandTests : ApiTestBase
{
    [Test]
    public async Task CanPlaceIOCOrder()
    {
        var uid = 100L;
        var symbol = 2;

        await AddUser(uid);
        await AddSymbol(symbol);

        var cmd = new PlaceOrderCommand(
            123L,
            DateTime.UtcNow,
            120.34m,
            37,
            OrderAction.BID,
            OrderType.IOC,
            uid,
            symbol);

        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = res.RootEvent;
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.EventType, Is.EqualTo(EngineEventType.REJECT));
            Assert.That(resEvent.Price, Is.EqualTo(120.34m));
            Assert.That(resEvent.Size, Is.EqualTo(37));
            Assert.That(resEvent.ActiveOrderCompleted, Is.True);
            Assert.That(resEvent.MatchedOrderCompleted, Is.False);
            Assert.That(resEvent.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(resEvent.MatchedOrderUid, Is.EqualTo(0L));
            Assert.That(resEvent.NextEvent, Is.Null);
        });
    }

    [Test]
    public async Task CanPlaceGTCOrderWithReport()
    {
        var uid = 100L;
        var symbol = 2;
        var orderId = 123L;

        await AddUser(uid);
        await AddSymbol(symbol);

        var cmd = new PlaceOrderCommand(
            orderId,
            DateTime.UtcNow,
            120.34m,
            37,
            OrderAction.BID,
            OrderType.GTC,
            uid,
            symbol);

        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var report = await GetUserReport(uid);
        Assert.That(report, Is.Not.Null);

        var ordersBySymbol = report.Orders[symbol];
        Assert.That(ordersBySymbol, Is.Not.Empty);

        var order = ordersBySymbol.First(o => o.OrderId == orderId);
        Assert.That(order, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(order.Price, Is.EqualTo(120.34m));
            Assert.That(order.Size, Is.EqualTo(37));
            Assert.That(order.Action, Is.EqualTo(OrderAction.BID));
        });
    }
}

