using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class CancelOrderCommandTests : ApiTestBase
{
    [Test]
    public async Task CanCancelOrder()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var cancel = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await Api.SendAsync(cancel);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = res.RootEvent;
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(resEvent.MatchedOrderCompleted, Is.False);
            Assert.That(resEvent.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(resEvent.MatchedOrderUid, Is.EqualTo(0L));
            Assert.That(resEvent.ActiveOrderCompleted, Is.True);
            Assert.That(resEvent.NextEvent, Is.Null);

            Assert.That(resEvent.Price, Is.EqualTo(23.45m));
            Assert.That(resEvent.Size, Is.EqualTo(34));
        });
    }

    [Test]
    public async Task CannotCancelNotExistingOrder()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var cancel = new CancelOrderCommand(orderId + 1, DateTime.UtcNow, uid, symbol);
        res = await Api.SendAsync(cancel);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_UNKNOWN_ORDER_ID));
    }

    [Test]
    public async Task CannotCancelOrderTwice()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var cancel = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await Api.SendAsync(cancel);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var cancel2 = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await Api.SendAsync(cancel2);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_UNKNOWN_ORDER_ID));
    }
}
