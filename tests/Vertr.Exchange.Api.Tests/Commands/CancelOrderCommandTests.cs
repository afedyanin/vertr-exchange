using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Shared.Enums;

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

        var orderId = 1236L;
        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34, orderId);

        var cancel = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var resEvent = GetReduceEvent(res.OrderId);
        Assert.That(resEvent, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(resEvent.OrderCompleted, Is.False);
            Assert.That(resEvent.OrderId, Is.EqualTo(orderId));
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

        var orderId = 2361L;
        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34, orderId);

        var cancel = new CancelOrderCommand(orderId + 1, DateTime.UtcNow, uid, symbol);
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

        var orderId = 3612L;
        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34, orderId);

        var cancel = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var cancel2 = new CancelOrderCommand(orderId, DateTime.UtcNow, uid, symbol);
        res = await SendAsync(cancel2);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_UNKNOWN_ORDER_ID));
    }
}
