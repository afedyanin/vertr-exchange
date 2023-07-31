using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;

namespace Vertr.Exchange.MatchingEngine;
internal class OrderBookCommandProcessor : IOrderBookCommandProcesor
{
    private readonly IOrderBook _orderBook;

    public OrderBookCommandProcessor(IOrderBook orderBook)
    {
        _orderBook = orderBook;
    }

    public CommandResultCode ProcessCommand(OrderCommand cmd)
    {
        var orderBookCommand = OrderBookCommandFactory.CreateOrderBookCommand(_orderBook, cmd);
        return orderBookCommand.Execute();
    }
}
