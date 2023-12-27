using Refit;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Shared.Reports;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ApiClient.Tests;

public abstract class TerminalApiTestBase
{
    protected ITerminalApiClient TerminalApiClient { get; private set; }

    protected ApiCommands ApiCommands { get; private set; }

    protected static Contracts.UserAccount BobAccount => StaticContext.UserAccounts.BobAccount;

    protected static Contracts.UserAccount AliceAccount => StaticContext.UserAccounts.AliceAccount;

    protected static Symbol Msft => StaticContext.Symbols.MSFT;

    protected static Currency MsftCurrrency => Msft.Currency;

    protected static decimal BobInitialAmount => BobAccount.Balances.GetByCurrency(MsftCurrrency)?.Amount ?? decimal.Zero;

    protected static decimal AliceInitialAmount => AliceAccount.Balances.GetByCurrency(MsftCurrrency)?.Amount ?? decimal.Zero;

    protected TerminalApiTestBase()
    {
        TerminalApiClient = RestService.For<ITerminalApiClient>("http://localhost:5010");
        ApiCommands = new ApiCommands(TerminalApiClient);
    }

    protected async Task Init()
    {
        await ApiCommands.Reset();
        await ApiCommands.AddSymbols(StaticContext.Symbols.All);
        await ApiCommands.AddUsers([BobAccount, AliceAccount]);
    }

    protected async Task<long> PlaceBobBid(decimal price, long qty)
    {
        return await PlaceOrder(BobAccount, price, qty);
    }

    protected async Task<long> PlaceAliceAsk(decimal price, long qty)
    {
        return await PlaceOrder(AliceAccount, price, qty * (-1));
    }

    protected async Task<long> PlaceOrder(Contracts.UserAccount account, decimal price, long qty)
    {
        var res = await ApiCommands.PlaceOrder(account.User, Msft, price, qty);
        ApiCommands.EnsureSuccess(res);
        return res!.OrderId;
    }

    protected void ValidateProfileAmounts(
        SingleUserReportResult? reportResult,
        decimal profileAmount,
        decimal expectedPnL,
        decimal expectedOpenVolume)
    {
        Assert.That(reportResult, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(reportResult.ExecutionStatus, Is.EqualTo(QueryExecutionStatus.OK));
            Assert.That(reportResult.UserStatus, Is.EqualTo(UserStatus.ACTIVE));
        });

        ValidateProfileAmount(reportResult, profileAmount);
        ValidateProfilePosition(reportResult, expectedPnL, expectedOpenVolume);
    }

    protected void ValidatePositionIsEmpty(SingleUserReportResult? report)
    {
        Assert.That(report, Is.Not.Null);
        report.Positions.TryGetValue(Msft.Id, out var position);
        Assert.That(position, Is.Null);
    }

    protected void ValidateProfileOrders(
        SingleUserReportResult? reportResult,
        int ordersCount)
    {
        Assert.That(reportResult, Is.Not.Null);

        foreach (var kvp in reportResult.Orders)
        {
            var orders = kvp.Value;

            if (kvp.Key == Msft.Id)
            {
                Assert.That(orders, Has.Length.EqualTo(ordersCount));
            }
            else
            {
                Assert.That(orders, Is.Empty);
            }
        }
    }

    protected void EnsureOrderBookContainsLevel(
        OrderBook[]? books,
        OrderAction action,
        decimal price,
        long qty,
        long ordersCount)
    {
        Assert.That(books, Is.Not.Null);

        var msftBook = books.FirstOrDefault(b => b.Symbol == Msft.Id);
        Assert.That(msftBook, Is.Not.Null);

        var records = action == OrderAction.ASK ? msftBook.Asks : msftBook.Bids;
        Assert.That(records, Is.Not.Null);

        var record = records.FirstOrDefault(r => r.Price == price);
        Assert.That(record, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(record.Price, Is.EqualTo(price));
            Assert.That(record.Volume, Is.EqualTo(qty));
            Assert.That(record.Orders, Is.EqualTo(ordersCount));
        });
    }

    protected void EnsureOrderBookDoesNotContainLevel(
        OrderBook[]? books,
        OrderAction action,
        decimal price)
    {
        Assert.That(books, Is.Not.Null);

        var msftBook = books.FirstOrDefault(b => b.Symbol == Msft.Id);
        Assert.That(msftBook, Is.Not.Null);

        var records = action == OrderAction.ASK ? msftBook.Asks : msftBook.Bids;
        Assert.That(records, Is.Not.Null);

        var record = records.FirstOrDefault(r => r.Price == price);
        Assert.That(record, Is.Null);
    }

    protected void ValidateOrder(
        OrderDto order,
        User user,
        OrderAction action,
        OrderType orderType,
        decimal price,
        long qty)
    {
        Assert.Multiple(() =>
        {
            Assert.That(order.UserId, Is.EqualTo(user.Id));
            Assert.That(order.Action, Is.EqualTo(action));
            Assert.That(order.Size, Is.EqualTo(qty));
            Assert.That(order.Price, Is.EqualTo(price));
            Assert.That(order.Symbol, Is.EqualTo(Msft.Id));
            Assert.That(order.OrderType, Is.EqualTo(orderType));
        });

        ValidatePlaceOrderEvent(order);
    }

    private void ValidatePlaceOrderEvent(OrderDto order)
    {
        Assert.That(order, Is.Not.Null);

        var events = order.OrderEvents;
        Assert.That(events, Is.Not.Empty);

        var placeEvent = events.Single(oe => oe.EventSource == OrderEventSource.PlaceOrderRequest);

        Assert.Multiple(() =>
        {
            Assert.That(placeEvent.OrderId, Is.EqualTo(order.OrderId));
            Assert.That(placeEvent.Action, Is.EqualTo(order.Action));
            Assert.That(placeEvent.Price, Is.EqualTo(order.Price));
            Assert.That(placeEvent.Volume, Is.EqualTo(order.Size));
        });
    }

    private void ValidateProfilePosition(
        SingleUserReportResult report,
        decimal expectedPnl,
        decimal expectedOpenVolume)
    {
        var position = report.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(position.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(position.OpenVolume, Is.EqualTo(expectedOpenVolume));
        });
    }

    private void ValidateProfileAmount(
        SingleUserReportResult report,
        decimal? expectedAmount)
    {
        var profileAmount = report.Accounts[Msft.Currency.Id];
        Assert.That(profileAmount, Is.EqualTo(expectedAmount));
    }
}
