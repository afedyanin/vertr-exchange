using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.MatchingEngine.Commands;
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
