using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

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
        res = await SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var reduced = await GetReduceEvent(orderId);
        Assert.That(reduced, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(reduced.OrderCompleted, Is.False);
            Assert.That(reduced.OrderId, Is.EqualTo(orderId));
            Assert.That(reduced.Uid, Is.EqualTo(uid));
            Assert.That(reduced.Price, Is.EqualTo(23.45m));
            Assert.That(reduced.ReducedVolume, Is.EqualTo(12));
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
        res = await SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, 5);
        res = await SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var reduced = await GetReduceEvent(orderId);
        Assert.That(reduced, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(reduced.OrderId, Is.EqualTo(orderId));
            Assert.That(reduced.Uid, Is.EqualTo(uid));
            Assert.That(reduced.OrderCompleted, Is.False);
            Assert.That(reduced.Price, Is.EqualTo(23.45m));
            Assert.That(reduced.ReducedVolume, Is.EqualTo(5));
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

        MessageHandler.Reset();
        var reduce = new ReduceOrderCommand(orderId, DateTime.UtcNow, uid, symbol, -12);
        res = await SendAsync(reduce);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE));
    }
}
