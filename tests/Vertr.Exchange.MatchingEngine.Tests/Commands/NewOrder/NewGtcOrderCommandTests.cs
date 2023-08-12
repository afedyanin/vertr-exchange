using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands.NewOrder;

[TestFixture(Category = "Unit")]
public class NewGtcOrderCommandTests
{
    [Test]
    public void PlaceWithoutMatching()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        var res = proc.ProcessCommand(cmd);
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.MatcherEvent, Is.Null);
        });
    }

    [Test]
    public void PlaceWithMatching()
    {
        var orderBook = new OrderBook();
        var ask1 = OrderStub.CreateAskOrder(18.58M, 17);
        var ask2 = OrderStub.CreateAskOrder(18.56M, 7);

        var bid = OrderStub.CreateBidOrder(19.23M, 40);

        orderBook.AddOrder(ask1);
        orderBook.AddOrder(ask2);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        var res = proc.ProcessCommand(cmd);

        var addedBid = orderBook.GetOrder(bid.OrderId);

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(ask1.Completed, Is.True);
            Assert.That(ask2.Completed, Is.True);
            Assert.That(addedBid!.Completed, Is.False);
            Assert.That(addedBid!.Size, Is.EqualTo(bid.Size));
            Assert.That(addedBid!.Filled, Is.EqualTo(ask1.Size + ask2.Size));
            Assert.That(cmd.MatcherEvent, Is.Not.Null);
            Assert.That(cmd.MatcherEvent!.EventType, Is.EqualTo(MatcherEventType.TRADE));
        });
    }
}
