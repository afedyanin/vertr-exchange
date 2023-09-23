using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class ReduceOrderCommandTests : ApiTestBase
{
    [Test]
    public async Task CanReduceOrder()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, 12);
        res = await Api.SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = res.RootEvent;
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(resEvent.MatchedOrderCompleted, Is.False);
            Assert.That(resEvent.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(resEvent.MatchedOrderUid, Is.EqualTo(0L));
            Assert.That(resEvent.ActiveOrderCompleted, Is.False);
            Assert.That(resEvent.NextEvent, Is.Null);

            Assert.That(resEvent.Price, Is.EqualTo(23.45m));
            Assert.That(resEvent.Size, Is.EqualTo(12));
        });
    }

    [Test]
    public async Task CanReduceOrderTwice()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, 12);
        res = await Api.SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, 5);
        res = await Api.SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = res.RootEvent;
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(resEvent.MatchedOrderCompleted, Is.False);
            Assert.That(resEvent.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(resEvent.MatchedOrderUid, Is.EqualTo(0L));
            Assert.That(resEvent.ActiveOrderCompleted, Is.False);
            Assert.That(resEvent.NextEvent, Is.Null);

            Assert.That(resEvent.Price, Is.EqualTo(23.45m));
            Assert.That(resEvent.Size, Is.EqualTo(5));
        });
    }

    [Test]
    public async Task CannotReduceNegativeSize()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, -12);
        res = await Api.SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE));
    }
}
