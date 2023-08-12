using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class MatchingUnsupportedCommand : OrderBookCommand
{
    public MatchingUnsupportedCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
    }
}
