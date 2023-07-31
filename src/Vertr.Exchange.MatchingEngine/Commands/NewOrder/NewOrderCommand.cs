using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal abstract class NewOrderCommand : OrderBookCommand
{
    protected NewOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }
}
