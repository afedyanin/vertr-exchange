using Microsoft.Extensions.Options;
using Vertr.Exchange.Common.Abstractions;

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

    public static OrderMatchingEngine Instance =>
        new OrderMatchingEngine(Options.Create(Configuration), CreateOrderBook);
}
