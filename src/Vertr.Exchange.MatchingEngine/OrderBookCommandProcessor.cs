using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;

namespace Vertr.Exchange.MatchingEngine;
internal sealed class OrderBookCommandProcessor : IOrderBookCommandProcesor
{
    private readonly IOrderBook _orderBook;

    public OrderBookCommandProcessor(IOrderBook orderBook)
    {
        _orderBook = orderBook;
    }

    public CommandResultCode ProcessCommand(OrderCommand cmd)
    {
        try
        {
            var orderBookCommand = OrderBookCommandFactory.CreateOrderBookCommand(_orderBook, cmd);
            return orderBookCommand.Execute();
        }
        catch
        {
            // TODO: Handle and log exception
            return CommandResultCode.DROP; // TODO: return CommandResultCode ?? 
        }
    }
}
