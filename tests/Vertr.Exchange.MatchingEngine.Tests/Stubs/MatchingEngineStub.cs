using Microsoft.Extensions.Options;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;
internal static class MatchingEngineStub
{
    public static MatchingEngineConfiguration Configuration =>
        new MatchingEngineConfiguration()
        {
            L2RefreshDepth = 100,
        };

    public static IOrderBook CreateOrderBook() =>
        new OrderBook();

    public static IOrderBookProvider OrderBookProvider =>
        new OrderBookProvider(CreateOrderBook);

    public static OrderMatchingEngine Instance =>
        new OrderMatchingEngine(OrderBookProvider, Options.Create(Configuration), LoggerStub.CreateConsoleLogger<OrderMatchingEngine>());
}
