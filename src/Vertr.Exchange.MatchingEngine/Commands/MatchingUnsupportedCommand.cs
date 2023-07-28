using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class MatchingUnsupportedCommand : OrderBookCommand
{
    public MatchingUnsupportedCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
    }
}
