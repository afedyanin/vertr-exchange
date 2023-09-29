using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;
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

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(bid.Remaining));
            Assert.That(cmd.EngineEvent!.Price, Is.EqualTo(bid.Price));
            Assert.That(cmd.EngineEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.EngineEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }
}
