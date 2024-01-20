using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Domain.Abstractions;
public interface IOrderEventService
{
    Task ProcessReduceEvent(ReduceEvent reduceEvent);

    Task ProcessRejectEvent(RejectEvent rejectEvent);

    Task ProcessTradeEvent(TradeEvent tradeEvent);
}
