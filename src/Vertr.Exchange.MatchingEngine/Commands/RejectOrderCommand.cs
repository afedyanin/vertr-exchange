using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class RejectOrderCommand : OrderBookCommand
{
    public RejectOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        //log.warn("Unsupported order type: {}", cmd);
        OrderCommand.MatcherEvent = CreateRejectEvent(OrderCommand.Price, OrderCommand.Size);
        return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND; // ??
    }
}
