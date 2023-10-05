using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Common.Abstractions;

public interface IMessageHandler
{
    Task CommandResult(ApiCommandResult apiCommandResult);

    Task TradeEvent(TradeEvent tradeEvent);

    Task RejectEvent(RejectEvent rejectEvent);

    Task ReduceEvent(ReduceEvent reduceEvent);

    Task OrderBook(OrderBook orderBook);
}
