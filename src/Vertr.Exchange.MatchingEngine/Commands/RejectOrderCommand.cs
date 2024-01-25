using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Events;

namespace Vertr.Exchange.Domain.MatchingEngine.Commands;
internal sealed class RejectOrderCommand(
    IOrderBook orderBook,
    OrderCommand cmd)
    : OrderBookCommand(orderBook, cmd)
{
    public override CommandResultCode Execute()
    {
        //log.warn("Unsupported order type: {}", cmd);
        EventsHelper.AttachRejectEvent(OrderCommand, OrderCommand.Price, OrderCommand.Size);
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND; // ??
    }
}
