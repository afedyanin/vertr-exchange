using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests.Commands;

[TestFixture(Category = "Unit")]
public class MarketDataSnapshotCommandTests
{
    [Test]
    public void CanGetOrderBook()
    {
        var orderBook = new OrderBook();
        var ask1 = OrderStub.CreateAskOrder(18.56M, 7);
        var ask2 = OrderStub.CreateAskOrder(19.17M, 23);
        var bid1 = OrderStub.CreateBidOrder(18.23M, 14, 10);
        var bid2 = OrderStub.CreateBidOrder(17.37M, 12, 1);
        var bid3 = OrderStub.CreateBidOrder(16.37M, 12, 1);

        orderBook.AddOrder(bid1);
        orderBook.AddOrder(bid2);
        orderBook.AddOrder(bid3);
        orderBook.AddOrder(ask1);
        orderBook.AddOrder(ask2);

        var cmd = OrderCommandStub.OrderBookRequest(100);

        var orderCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, 100);
        var res = orderCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.MarketData, Is.Not.Null);
            Assert.That(cmd.MarketData!.AskOrders, Has.Length.EqualTo(2));
            Assert.That(cmd.MarketData!.BidOrders, Has.Length.EqualTo(3));
        });
    }
}
