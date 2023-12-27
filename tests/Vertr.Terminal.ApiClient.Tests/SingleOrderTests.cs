using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System")]
public class SingleOrderTests : TerminalApiTestBase
{
    [SetUp]
    public async Task SetUp()
    {
        await Init();
    }

    [Test]
    public async Task ExchangeWithZeroOrdersHasValidState()
    {
        var profile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        ValidatePositionIsEmpty(profile);

        var books = await ApiCommands.GetOrderBooks();

        foreach (var book in books)
        {
            Assert.Multiple(() =>
            {
                Assert.That(book.Asks, Is.Empty);
                Assert.That(book.Bids, Is.Empty);
            });
        }

        var trades = await ApiCommands.GetTrades();
        Assert.That(trades, Is.Empty);

        var orders = await ApiCommands.GetOrders();
        Assert.That(orders, Is.Empty);
    }

    [Test]
    public async Task ExchangeWithSingleOrderHasValidState()
    {
        const decimal singleOrderPrice = 100m;
        const long singleOrderQty = 24;

        var orderId = await PlaceBobBid(singleOrderPrice, singleOrderQty);

        var profile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        ValidatePositionIsEmpty(profile);
        ValidateProfileOrders(profile, 1);

        var books = await ApiCommands.GetOrderBooks();
        EnsureOrderBookContainsLevel(books, OrderAction.BID, singleOrderPrice, singleOrderQty, 1);

        var trades = await ApiCommands.GetTrades();
        Assert.That(trades, Is.Empty);

        var orders = await ApiCommands.GetOrders();
        Assert.That(orders.Count, Is.EqualTo(1));

        var order = orders.First(o => o.OrderId == orderId);

        ValidateOrder(
            order,
            BobAccount.User,
            OrderAction.BID,
            OrderType.GTC,
            singleOrderPrice,
            singleOrderQty);
    }

    [Test]
    public async Task ExchangeWithTwoOrdersHasValidState()
    {
        const decimal bidPrice = 100m;
        const long bidQty = 24;
        const decimal askPrice = 120m;
        const long askQty = 26;

        var bobOrderId = await PlaceBobBid(bidPrice, bidQty);
        var aliceOrderId = await PlaceAliceAsk(askPrice, askQty);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        ValidatePositionIsEmpty(bobProfile);
        ValidateProfileOrders(bobProfile, 1);

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        ValidatePositionIsEmpty(aliceProfile);
        ValidateProfileOrders(aliceProfile, 1);

        var books = await ApiCommands.GetOrderBooks();
        EnsureOrderBookContainsLevel(books, OrderAction.BID, bidPrice, bidQty, 1);
        EnsureOrderBookContainsLevel(books, OrderAction.ASK, askPrice, askQty, 1);

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

        var trades = await ApiCommands.GetTrades();
        Assert.That(trades, Is.Empty);
    }
}
