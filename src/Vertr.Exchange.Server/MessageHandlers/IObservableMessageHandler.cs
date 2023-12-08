using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.MessageHandlers;

public interface IObservableMessageHandler
{
    IObservable<ApiCommandResult> ApiCommandResultStream();

    IObservable<OrderBook> OrderBookStream();

    IObservable<ReduceEvent> ReduceEventStream();

    IObservable<RejectEvent> RejectEventStream();

    IObservable<TradeEvent> TradeEventStream();
}
