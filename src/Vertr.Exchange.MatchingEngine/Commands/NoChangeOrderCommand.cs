using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class NoChangeOrderCommand : OrderBookCommand
{
    public NoChangeOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        return OrderCommand.ResultCode;
    }
}
