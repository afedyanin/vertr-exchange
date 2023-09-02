using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class RejectOrderCommand : OrderBookCommand
{
    public RejectOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        //log.warn("Unsupported order type: {}", cmd);
        EventsHelper.AttachRejectEvent(OrderCommand, OrderCommand.Price, OrderCommand.Size);
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND; // ??
    }
}
