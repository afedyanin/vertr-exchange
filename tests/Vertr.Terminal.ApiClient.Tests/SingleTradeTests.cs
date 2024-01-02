using Vertr.Exchange.Contracts;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System", Explicit = true)]
public class SingleTradeTests : TerminalApiTestBase
{
    [SetUp]
    public async Task Setup()
    {
        await Init();
    }

    [TestCase(124, 24, 120, 26)]
    [TestCase(124, 28, 120, 26)]
    [TestCase(124, 26, 120, 26)]
    public async Task ExchangeHasValidStateAfterSingleTrade(
        decimal bidPrice,
        long bidQty,
        decimal askPrice,
        long askQty)
    {
        var minQty = Math.Min(bidQty, askQty);

        var bobOrderId = await PlaceBobBid(bidPrice, bidQty);
        var aliceOrderId = await PlaceAliceAsk(askPrice, askQty);

        // TODO: Implement amount withdrawal after order execution
        // var expBidAmount = BobInitialAmount - (bidPrice * bidQty);
        // ValidateProfileAmounts(bobProfile, expBidAmount, decimal.Zero, minQty);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        ValidateProfileAmounts(bobProfile, BobInitialAmount, decimal.Zero, minQty);

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        ValidateProfileAmounts(aliceProfile, AliceInitialAmount, decimal.Zero, minQty);

        var books = await ApiCommands.GetOrderBooks();

        if (bidQty < askQty)
        {
            ValidateProfileOrders(bobProfile, 0);
            ValidateProfileOrders(aliceProfile, 1);
            EnsureOrderBookDoesNotContainLevel(books, OrderAction.BID, bidPrice);
            EnsureOrderBookContainsLevel(books, OrderAction.ASK, askPrice, Math.Abs(askQty - bidQty), 1);
        }
        else if (bidQty > askQty)
        {
            ValidateProfileOrders(bobProfile, 1);
            ValidateProfileOrders(aliceProfile, 0);
            EnsureOrderBookContainsLevel(books, OrderAction.BID, bidPrice, Math.Abs(askQty - bidQty), 1);
            EnsureOrderBookDoesNotContainLevel(books, OrderAction.ASK, askPrice);
        }
        else
        {
            ValidateProfileOrders(bobProfile, 0);
            ValidateProfileOrders(aliceProfile, 0);
            EnsureOrderBookDoesNotContainLevel(books, OrderAction.BID, bidPrice);
            EnsureOrderBookDoesNotContainLevel(books, OrderAction.ASK, askPrice);
        }

        var orders = await ApiCommands.GetOrders();
        Assert.That(orders.Count, Is.EqualTo(2));

        var bobOrder = orders.First(o => o.OrderId == bobOrderId);
        var aliceOrder = orders.First(o => o.OrderId == aliceOrderId);

        ValidateOrder(
            bobOrder,
            BobAccount.User,
            OrderAction.BID,
            OrderType.GTC,
            bidPrice,
            bidQty);

        ValidateOrder(
            aliceOrder,
            AliceAccount.User,
            OrderAction.ASK,
            OrderType.GTC,
            askPrice,
            askQty);

        ValidateOrderEvents(bobOrder, aliceOrder);

        var trades = await ApiCommands.GetTrades();
        Assert.That(trades, Is.Not.Empty);

        foreach (var trade in trades)
        {
            ValidateTradeEvent(bobOrder, aliceOrder, trade);
        }
    }

    private void ValidateTradeEvent(
        Contracts.OrderDto makerOrder,
        Contracts.OrderDto takerOrder,
        TradeEvent tradeEvent)
    {
        Assert.Multiple(() =>
        {
            Assert.That(takerOrder, Is.Not.Null);
            Assert.That(makerOrder, Is.Not.Null);
        });

        var minQty = Math.Min(takerOrder.Size, makerOrder.Size);

        Assert.Multiple(() =>
        {
            Assert.That(tradeEvent.TakerOrderId, Is.EqualTo(takerOrder.OrderId));
            Assert.That(tradeEvent.TakerAction, Is.EqualTo(takerOrder.Action));
            Assert.That(tradeEvent.TotalVolume, Is.EqualTo(minQty));
            Assert.That(tradeEvent.TakeOrderCompleted, Is.EqualTo(minQty == takerOrder.Size));
        });

        var makerEvent = tradeEvent.Trades.First();

        Assert.Multiple(() =>
        {
            Assert.That(makerEvent.MakerOrderId, Is.EqualTo(makerOrder.OrderId));
            Assert.That(makerEvent.Price, Is.EqualTo(makerOrder.Price));
            Assert.That(makerEvent.Volume, Is.EqualTo(minQty));
            Assert.That(makerEvent.MakerOrderCompleted, Is.EqualTo(minQty == makerOrder.Size));
        });
    }

    private void ValidateOrderEvents(
        Contracts.OrderDto makerOrder,
        Contracts.OrderDto takerOrder)
    {
        Assert.Multiple(() =>
        {
            Assert.That(takerOrder, Is.Not.Null);
            Assert.That(makerOrder, Is.Not.Null);
        });

        var minQty = Math.Min(takerOrder.Size, makerOrder.Size);

        var makerEvent = makerOrder.OrderEvents.Single(oe => oe.EventSource == OrderEventSource.MakerTradeEvent);

        Assert.Multiple(() =>
        {
            Assert.That(makerEvent.OrderId, Is.EqualTo(makerOrder.OrderId));
            Assert.That(makerEvent.Action, Is.EqualTo(makerOrder.Action));
            Assert.That(makerEvent.Price, Is.EqualTo(makerOrder.Price));
            Assert.That(makerEvent.Volume, Is.EqualTo(minQty));
            Assert.That(makerEvent.OrderCompleted, Is.EqualTo(minQty == makerOrder.Size));
        });

        var takerEvent = takerOrder.OrderEvents.Single(oe => oe.EventSource == OrderEventSource.TakerTradeEvent);

        Assert.Multiple(() =>
        {
            Assert.That(takerEvent.OrderId, Is.EqualTo(takerOrder.OrderId));
            Assert.That(takerEvent.Action, Is.EqualTo(takerOrder.Action));
            Assert.That(takerEvent.Price, Is.EqualTo(takerOrder.Price));
            Assert.That(takerEvent.Volume, Is.EqualTo(minQty));
            Assert.That(takerEvent.OrderCompleted, Is.EqualTo(minQty == takerOrder.Size));
        });
    }
}
