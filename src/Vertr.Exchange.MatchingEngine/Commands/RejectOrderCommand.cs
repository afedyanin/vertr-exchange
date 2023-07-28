using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class RejectOrderCommand : OrderBookCommand
{
    public RejectOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        //log.warn("Unsupported order type: {}", cmd);
        _eventFactory.AttachRejectEvent(OrderCommand, OrderCommand.Size);
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND; // ??
    }
}
