using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class MatchingUnsupportedCommand(
    IOrderBook orderBook,
    OrderCommand cmd)
    : OrderBookCommand(orderBook, cmd)
{
    public override CommandResultCode Execute()
    {
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
    }
}
