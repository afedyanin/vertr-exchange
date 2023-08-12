using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands;

[TestFixture(Category = "Unit")]
public class ReduceOrderCommandTests
{
    [Test]
    public void ProcessReduceBasic()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 14); // remaining = 27-14 = 13
        orderBook.AddOrder(bid);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.Reduce(
            bid.OrderId,
            bid.Uid,
            7);

        // Size = 27-7 = 20
        // Filled = 14
        // Remaining = 6

        var res = proc.ProcessCommand(cmd);
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(20));
            Assert.That(bid.Price, Is.EqualTo(45.23M));
            Assert.That(bid.Filled, Is.EqualTo(14));
            Assert.That(cmd.MatcherEvent, Is.Not.Null);
            Assert.That(cmd.MatcherEvent!.EventType, Is.EqualTo(MatcherEventType.REDUCE));
            Assert.That(cmd.MatcherEvent!.Size, Is.EqualTo(7));
            Assert.That(cmd.MatcherEvent!.Price, Is.EqualTo(45.23M));
            Assert.That(cmd.MatcherEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.MatcherEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }

    [Test]
    public void ProcessReduceRemainingLessThanReduce()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 17); // remaining = 27-17 = 10
        orderBook.AddOrder(bid);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.Reduce(
            bid.OrderId,
            bid.Uid,
            19);

        // Size = 27 - 10 = 17
        // Filled = 17
        // Remaining = 0

        var res = proc.ProcessCommand(cmd);
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(17));
            Assert.That(bid.Price, Is.EqualTo(45.23M));
            Assert.That(bid.Filled, Is.EqualTo(17));
            Assert.That(cmd.MatcherEvent, Is.Not.Null);
            Assert.That(cmd.MatcherEvent!.EventType, Is.EqualTo(MatcherEventType.REDUCE));
            Assert.That(cmd.MatcherEvent!.Size, Is.EqualTo(10));
            Assert.That(cmd.MatcherEvent!.Price, Is.EqualTo(45.23M));
            Assert.That(cmd.MatcherEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.MatcherEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }
}
