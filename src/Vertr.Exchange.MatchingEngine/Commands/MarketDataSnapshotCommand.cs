using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class MarketDataSnapshotCommand(
    IOrderBook orderBook,
    OrderCommand cmd,
    long sequence) : OrderBookCommand(orderBook, cmd)
{
    private readonly long _sequence = sequence;

    public override CommandResultCode Execute()
    {
        var size = (int)OrderCommand.Size;
        OrderCommand.MarketData = OrderBook.GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue, _sequence);
        return CommandResultCode.SUCCESS;
    }
}
