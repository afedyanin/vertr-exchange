using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands;

[TestFixture(Category = "Unit")]
public class RejectOrderCommandTests
{
    [Test]
    public void ProcessReduceBasic()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 14); // remaining = 27-14 = 13

        var cmd = OrderCommandStub.Reject(
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.MATCHING_UNSUPPORTED_COMMAND));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.REJECT));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(bid.Size));
            Assert.That(cmd.EngineEvent!.Price, Is.EqualTo(bid.Price));
            Assert.That(cmd.EngineEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.EngineEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }
}
