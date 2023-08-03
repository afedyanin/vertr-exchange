using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;

internal abstract class OrderBookCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IOrderBook OrderBook { get; }

    protected OrderBookCommand(
        IOrderBook orderBook,
        OrderCommand cmd)
    {
        OrderBook = orderBook;
        OrderCommand = cmd;
    }
    public abstract CommandResultCode Execute();
}
