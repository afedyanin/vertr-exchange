using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands;

[TestFixture(Category = "Unit")]
public class MoveOrderCommandTests
{
    [Test]
    public void ProcessMoveWithoutMatching()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27);
        orderBook.AddOrder(bid);

        var cmd = OrderCommandStub.MoveOrder(
            bid.OrderId,
            bid.Uid,
            47.97M,
            23);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(23));
            Assert.That(bid.Price, Is.EqualTo(47.97M));
            Assert.That(bid.Filled, Is.EqualTo(0));
        });
    }

    [Test]
    public void ProcessMoveWithoutMatchingWithInvalidSize()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27, 13);
        orderBook.AddOrder(bid);

        var cmd = OrderCommandStub.MoveOrder(
            bid.OrderId,
            bid.Uid,
            47.97M,
            3);

        Assert.Throws<InvalidOperationException>(() =>
        {
            var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
            var res = orderCommand.Execute();
        });
    }

    [Test]
    public void ProcessMoveWithMatchingSuccess()
    {
        var orderBook = new OrderBook();
        var ask = OrderStub.CreateAskOrder(18.56M, 7);
        var bid = OrderStub.CreateBidOrder(18.23M, 14, 10);

        orderBook.AddOrder(bid);
        orderBook.AddOrder(ask);

        var cmd = OrderCommandStub.MoveOrder(
            bid.OrderId,
            bid.Uid,
            18.60M,
            23);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(23));
            Assert.That(bid.Price, Is.EqualTo(18.60M));
            Assert.That(ask.Completed, Is.True);
            Assert.That(bid.Completed, Is.False);
            Assert.That(bid.Size, Is.EqualTo(23));
            Assert.That(bid.Filled, Is.EqualTo(17));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.TRADE));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(7));
            Assert.That(cmd.EngineEvent!.Price, Is.EqualTo(18.56M));
            Assert.That(cmd.EngineEvent!.MatchedOrderId, Is.EqualTo(ask.OrderId));
            Assert.That(cmd.EngineEvent!.MatchedOrderUid, Is.EqualTo(ask.Uid));
        });
    }
}
