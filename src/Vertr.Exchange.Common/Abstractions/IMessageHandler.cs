using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Common.Abstractions;

public interface IMessageHandler
{
    void CommandResult(ApiCommandResult apiCommandResult);

    void TradeEvent(TradeEvent tradeEvent);

    void RejectEvent(RejectEvent rejectEvent);

    void ReduceEvent(ReduceEvent reduceEvent);

    void OrderBook(OrderBook orderBook);
}
