using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;
using Vertr.Exchange.Domain.MatchingEngine.OrderBooks;
using Vertr.Exchange.Domain.MatchingEngine.Commands;
using Vertr.Exchange.Domain.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.Domain.MatchingEngine.Tests.Commands.NewOrder;

[TestFixture(Category = "Unit")]
public class NewIocOrderCommandTests
{
    [Test]
    public void PlaceWithoutMatching()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.23M, 27);

        var cmd = OrderCommandStub.IocOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.REJECT));
            Assert.That(cmd.EngineEvent!.Size, Is.EqualTo(27));
        });
    }

    [Test]
    public void PlaceWithMatching()
    {
        var orderBook = new OrderBook();
        var ask1 = OrderStub.CreateAskOrder(18.58M, 17);
        var ask2 = OrderStub.CreateAskOrder(18.56M, 7);

        var bid = OrderStub.CreateBidOrder(19.23M, 23);

        orderBook.AddOrder(ask1);
        orderBook.AddOrder(ask2);

        var cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(ask2.Completed, Is.True);
            Assert.That(ask1.Completed, Is.False);
            Assert.That(ask1.Remaining, Is.EqualTo(1));
            Assert.That(cmd.EngineEvent, Is.Not.Null);
            Assert.That(cmd.EngineEvent!.EventType, Is.EqualTo(EngineEventType.TRADE));
        });
    }
}
