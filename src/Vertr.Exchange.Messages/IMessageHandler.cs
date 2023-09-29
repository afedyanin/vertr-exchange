namespace Vertr.Exchange.Messages;

public interface IMessageHandler
{
    void CommandResult(ApiCommandResult apiCommandResult);

    void TradeEvent(TradeEvent tradeEvent);

    void RejectEvent(RejectEvent rejectEvent);

    void ReduceEvent(ReduceEvent reduceEvent);

    void OrderBook(OrderBook orderBook);
}
