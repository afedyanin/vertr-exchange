using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;
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

        var cmd = OrderCommandStub.Reduce(
            bid.OrderId,
            bid.Uid,
            7);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        // Size = 27-7 = 20
        // Filled = 14
        // Remaining = 6

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(20));
            Assert.That(bid.Price, Is.EqualTo(45.23M));
            Assert.That(bid.Filled, Is.EqualTo(14));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(7));
            Assert.That(cmd.EngineEvent!.Price, Is.EqualTo(45.23M));
            Assert.That(cmd.EngineEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.EngineEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }

    [Test]
    public void ProcessReduceRemainingLessThanReduce()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 17); // remaining = 27-17 = 10
        orderBook.AddOrder(bid);

        var cmd = OrderCommandStub.Reduce(
            bid.OrderId,
            bid.Uid,
            19);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        // Size = 27 - 10 = 17
        // Filled = 17
        // Remaining = 0

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(17));
            Assert.That(bid.Price, Is.EqualTo(45.23M));
            Assert.That(bid.Filled, Is.EqualTo(17));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.REDUCE));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(10));
            Assert.That(cmd.EngineEvent!.Price, Is.EqualTo(45.23M));
            Assert.That(cmd.EngineEvent!.MatchedOrderId, Is.EqualTo(0L));
            Assert.That(cmd.EngineEvent!.MatchedOrderUid, Is.EqualTo(0L));
        });
    }
}
