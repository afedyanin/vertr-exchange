using Vertr.Exchange.Domain.Common.Messages;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IMessageHandler
{
    Guid Id { get; }

    void CommandResult(ApiCommandResult apiCommandResult);

    void TradeEvent(TradeEvent tradeEvent);

    void RejectEvent(RejectEvent rejectEvent);

    void ReduceEvent(ReduceEvent reduceEvent);

    void OrderBook(OrderBook orderBook);
}
