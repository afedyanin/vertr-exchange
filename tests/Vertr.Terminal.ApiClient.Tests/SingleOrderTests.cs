using Refit;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Shared.Reports;
using Vertr.Exchange.Shared.Reports.Dtos;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System")]
public class SingleOrderTests
{
    private const decimal SingleOrderPrice = 100m;
    private const long SingleOrderQty = 24;

    private ApiCommands _api;
    private static readonly UserAccount _account = StaticContext.UserAccounts.BobAccount;

    [SetUp]
    public async Task SetUp()
    {
        var apiClient = RestService.For<ITerminalApiClient>("http://localhost:5010");
        _api = new ApiCommands(apiClient);

        await _api.Reset();
        await _api.AddSymbols(StaticContext.Symbols.All);
        await _api.AddUsers([_account]);
    }

    [Test]
    public async Task ExchangeWithZeroOrdersHasValidState()
    {
        var profile = await _api.GetSingleUserReport(_account.User);
        ValidateZeroOrderUserProfile(profile, _account);

        var books = await _api.GetOrderBooks();
        foreach (var book in books)
        {
            Assert.Multiple(() =>
            {
                Assert.That(book.Asks, Is.Empty);
                Assert.That(book.Bids, Is.Empty);
            });
        }

        var trades = await _api.GetTrades();
        Assert.That(trades, Is.Empty);

        var orders = await _api.GetOrders();
        Assert.That(orders, Is.Empty);
    }

    [Test]
    public async Task ExchangeWithSingleOrderHasValidState()
    {
        var account = StaticContext.UserAccounts.BobAccount;
        var msft = StaticContext.Symbols.MSFT;
        var orderId = await PlaceSingleOrder(account, msft);

        var profile = await _api.GetSingleUserReport(account.User);
        ValidateSingleOrderUserProfile(profile, account, msft);

        var books = await _api.GetOrderBooks();
        foreach (var book in books)
        {
            if (book.Symbol == msft.Id)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(book.Asks.Count, Is.EqualTo(0));
                    Assert.That(book.Bids.Count, Is.EqualTo(1));
                });

                var record = book.Bids[0];
                Assert.Multiple(() =>
                {
                    Assert.That(record.Price, Is.EqualTo(SingleOrderPrice));
                    Assert.That(record.Volume, Is.EqualTo(SingleOrderQty));
                    Assert.That(record.Orders, Is.EqualTo(1));
                });
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(book.Asks, Is.Empty);
                    Assert.That(book.Bids, Is.Empty);
                });
            }
        }

        var trades = await _api.GetTrades();
        Assert.That(trades, Is.Empty);

        var orders = await _api.GetOrders();
        Assert.That(orders.Count, Is.EqualTo(1));

        var order = orders[0];
        Assert.Multiple(() =>
        {
            Assert.That(order.UserId, Is.EqualTo(_account.User.Id));
            Assert.That(order.Action, Is.EqualTo(OrderAction.BID));
            Assert.That(order.Size, Is.EqualTo(SingleOrderQty));
            Assert.That(order.Price, Is.EqualTo(SingleOrderPrice));
            Assert.That(order.Symbol, Is.EqualTo(msft.Id));
            Assert.That(order.OrderType, Is.EqualTo(OrderType.GTC));
            Assert.That(order.OrderId, Is.EqualTo(orderId));
            Assert.That(order.OrderEvents.Count, Is.EqualTo(1));
        });

        var oevt = order.OrderEvents[0];
        Assert.Multiple(() =>
        {
            Assert.That(oevt.Action, Is.EqualTo(OrderAction.BID));
            Assert.That(oevt.EventSource, Is.EqualTo(OrderEventSource.PlaceOrderRequest));
            Assert.That(oevt.Price, Is.EqualTo(SingleOrderPrice));
            Assert.That(oevt.Volume, Is.EqualTo(SingleOrderQty));
            Assert.That(oevt.OrderId, Is.EqualTo(orderId));
        });
    }

    private async Task<long> PlaceSingleOrder(UserAccount account, Symbol symbol)
    {
        var res = await _api.PlaceOrder(account.User, symbol, SingleOrderPrice, SingleOrderQty);
        _api.EnsureSuccess(res);
        return res!.OrderId;
    }

    private void ValidateSingleOrderUserProfile(
        SingleUserReportResult? reportResult,
        UserAccount account,
        Symbol orderSymbol)
    {
        Assert.That(reportResult, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(reportResult.ExecutionStatus, Is.EqualTo(QueryExecutionStatus.OK));
            Assert.That(reportResult.UserStatus, Is.EqualTo(UserStatus.ACTIVE));
        });

        foreach (var currency in StaticContext.Currencies.All)
        {
            var amountInitial = account.Balances.GetByCurrency(currency)?.Amount;
            ValidateAccount(reportResult, currency, amountInitial);
        }

        foreach (var kvp in reportResult.Positions)
        {
            var symbol = StaticContext.Symbols.All.GetById(kvp.Key);
            var position = kvp.Value;
            Assert.That(position.Symbol, Is.EqualTo(symbol!.Id));

            ValidatePosition(position);
        }

        foreach (var kvp in reportResult.Orders)
        {
            var symbol = StaticContext.Symbols.All.GetById(kvp.Key);

            var orders = kvp.Value;

            if (symbol == orderSymbol)
            {
                Assert.That(orders, Has.Length.EqualTo(1));
            }
            else
            {
                Assert.That(orders, Is.Empty);
            }
        }
    }

    private void ValidateZeroOrderUserProfile(SingleUserReportResult? reportResult, UserAccount account)
    {
        Assert.That(reportResult, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(reportResult.ExecutionStatus, Is.EqualTo(QueryExecutionStatus.OK));
            Assert.That(reportResult.UserStatus, Is.EqualTo(UserStatus.ACTIVE));
        });

        foreach (var currency in StaticContext.Currencies.All)
        {
            var amountInitial = account.Balances.GetByCurrency(currency)?.Amount;
            ValidateAccount(reportResult, currency, amountInitial);
        }

        foreach (var kvp in reportResult.Positions)
        {
            var symbol = StaticContext.Symbols.All.GetById(kvp.Key);
            var position = kvp.Value;
            Assert.That(position.Symbol, Is.EqualTo(symbol!.Id));

            ValidatePosition(position);
        }

        foreach (var kvp in reportResult.Orders)
        {
            var symbol = StaticContext.Symbols.All.GetById(kvp.Key);
            var orders = kvp.Value;
            Assert.That(orders, Is.Empty);
        }
    }

    private void ValidateAccount(
        SingleUserReportResult report,
        Currency currency,
        decimal? expectedAmount)
    {
        var amountProfile = report.Accounts[currency.Id];
        Assert.That(amountProfile, Is.EqualTo(expectedAmount));
    }

    private void ValidatePosition(
        PositionDto position,
        decimal pnl = decimal.Zero,
        decimal openVolume = decimal.Zero)
    {
        Assert.Multiple(() =>
        {
            Assert.That(position.RealizedPnL, Is.EqualTo(pnl));
            Assert.That(position.OpenVolume, Is.EqualTo(openVolume));
        });
    }
}
