using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Application.Commands;

namespace Vertr.Exchange.Application.Tests.Commands;

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

        var cancel = new CancelOrderCommand(res.OrderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = await GetReduceEvent(res.OrderId);
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.OrderCompleted, Is.True);
            Assert.That(resEvent.OrderId, Is.EqualTo(res.OrderId));
            Assert.That(resEvent.Price, Is.EqualTo(23.45m));
            Assert.That(resEvent.Uid, Is.EqualTo(uid));
            Assert.That(resEvent.ReducedVolume, Is.EqualTo(34));
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
        var cancel = new CancelOrderCommand(res.OrderId + 100_000, DateTime.UtcNow, uid, symbol);

        res = await SendAsync(cancel);

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

        MessageHandler.Reset();
        var cancel = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        MessageHandler.Reset();
        var cancel2 = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel2);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_UNKNOWN_ORDER_ID));
    }
}
