using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands;

[TestFixture(Category = "Unit")]
public class CancelOrderCommandTests
{
    [Test]
    public void ProcessCancel()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 14); // remaining = 27-14 = 13

        orderBook.AddOrder(bid);

        var cmd = OrderCommandStub.Cancel(
            bid.OrderId,
            bid.Uid);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.MatcherEvent, Is.Not.Null);
            Assert.That(cmd.MatcherEvent!.EventType, Is.EqualTo(MatcherEventType.REDUCE));
            Assert.That(cmd.MatcherEvent!.Size, Is.EqualTo(bid.Remaining));
            Assert.That(cmd.MatcherEvent!.Price, Is.EqualTo(bid.Price));
            Assert.That(cmd.MatcherEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.MatcherEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }
}
