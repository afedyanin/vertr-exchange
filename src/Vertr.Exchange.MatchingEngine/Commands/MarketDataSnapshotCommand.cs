using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class MarketDataSnapshotCommand : OrderBookCommand
{
    public MarketDataSnapshotCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var size = (int)OrderCommand.Size;
        OrderCommand.MarketData = OrderBook.GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue);
        return CommandResultCode.SUCCESS;
    }
}
