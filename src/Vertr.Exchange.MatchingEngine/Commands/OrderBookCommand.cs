using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands;

internal abstract class OrderBookCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IOrderBook OrderBook { get; }

    public IOrder? Order { get; }

    public bool HasValidOrder
        => Order is not null &&
           Order.Uid == OrderCommand.Uid;

    protected OrderBookCommand(
        IOrderBook orderBook,
        OrderCommand cmd)
    {
        OrderBook = orderBook;
        OrderCommand = cmd;
        Order = OrderBook.GetOrder(OrderCommand.OrderId);
    }
    public abstract CommandResultCode Execute();

    protected void UpdateCommandAction()
    {
        if (Order is null)
        {
            return;
        }

        OrderCommand.Action = Order.Action;
    }

    protected void AttachTradeEvents(IEnumerable<IMatcherTradeEvent> tradeEvents)
    {
        OrderCommand.AttachMatcherEvents(tradeEvents);
    }
}
