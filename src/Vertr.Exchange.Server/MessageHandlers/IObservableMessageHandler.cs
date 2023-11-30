using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Server.MessageHandlers;

public interface IObservableMessageHandler : IMessageHandler
{
    IObservable<ApiCommandResult> ApiCommandResultStream();

    IObservable<OrderBook> OrderBookStream();

    IObservable<ReduceEvent> ReduceEventStream();

    IObservable<RejectEvent> RejectEventStream();

    IObservable<TradeEvent> TradeEventStream();
}
