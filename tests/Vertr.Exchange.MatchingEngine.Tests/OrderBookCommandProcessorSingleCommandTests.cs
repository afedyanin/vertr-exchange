using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderBookCommandProcessorSingleCommandTests
{
    [Test]
    public void ProcessMoveWithoutMatching()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27);
        orderBook.AddOrder(bid);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.MoveOrder(
            bid.OrderId,
            bid.Uid,
            47.97M,
            23);

        var res = proc.ProcessCommand(cmd);
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(23));
            Assert.That(bid.Price, Is.EqualTo(47.97M));
            Assert.That(bid.Filled, Is.EqualTo(0));
        });
    }

    [Test]
    public void ProcessMoveWithoutMatchingWithInvalidAction()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27);
        orderBook.AddOrder(bid);

        var proc = new OrderBookCommandProcessor(orderBook);

        var cmd = OrderCommandStub.MoveOrder(
            bid.OrderId,
            bid.Uid,
            47.97M,
            23);

        var res = proc.ProcessCommand(cmd);
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(bid.Size, Is.EqualTo(23));
            Assert.That(bid.Price, Is.EqualTo(47.97M));
            Assert.That(bid.Filled, Is.EqualTo(0));
        });
    }
}
