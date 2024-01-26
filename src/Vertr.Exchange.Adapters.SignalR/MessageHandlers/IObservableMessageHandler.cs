using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Adapters.SignalR.MessageHandlers;

public interface IObservableMessageHandler
{
    IObservable<ApiCommandResult> ApiCommandResultStream();

    IObservable<OrderBook> OrderBookStream();

    IObservable<ReduceEvent> ReduceEventStream();

    IObservable<RejectEvent> RejectEventStream();

    IObservable<TradeEvent> TradeEventStream();
}
