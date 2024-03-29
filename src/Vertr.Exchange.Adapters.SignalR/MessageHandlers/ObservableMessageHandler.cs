using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Adapters.SignalR.Extensions;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Adapters.SignalR.MessageHandlers;

public class ObservableMessageHandler : IObservableMessageHandler, Application.Messages.IMessageHandler, IDisposable
{
    private readonly Subject<ApiCommandResult> _commandResultSubject = new Subject<ApiCommandResult>();
    private readonly Subject<OrderBook> _orderBookSubject = new Subject<OrderBook>();
    private readonly Subject<ReduceEvent> _reduceEventSubject = new Subject<ReduceEvent>();
    private readonly Subject<RejectEvent> _rejectEventSubject = new Subject<RejectEvent>();
    private readonly Subject<TradeEvent> _tradeEventSubject = new Subject<TradeEvent>();

    private bool _disposed;

    public Guid Id { get; }

    private readonly ILogger<ObservableMessageHandler> _logger;

    public ObservableMessageHandler(ILogger<ObservableMessageHandler> logger)
    {
        _logger = logger;
        Id = Guid.NewGuid();
        _logger.LogInformation("Starting ObservableMessageHandler. Identity={id}", Id.ToString());
    }

    public IObservable<ApiCommandResult> ApiCommandResultStream() => _commandResultSubject;

    public IObservable<OrderBook> OrderBookStream() => _orderBookSubject;

    public IObservable<ReduceEvent> ReduceEventStream() => _reduceEventSubject;

    public IObservable<RejectEvent> RejectEventStream() => _rejectEventSubject;

    public IObservable<TradeEvent> TradeEventStream() => _tradeEventSubject;

    public void CommandResult(Application.Messages.ApiCommandResult apiCommandResult)
    {
        _logger.LogDebug($"CommandResult received: OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
        _commandResultSubject.OnNext(apiCommandResult.ToDto());
    }

    public void OrderBook(Application.Messages.OrderBook orderBook)
    {
        _logger.LogDebug($"OrderBook received: Symbol={orderBook.Symbol}");
        _orderBookSubject.OnNext(orderBook.ToDto());
    }

    public void ReduceEvent(Application.Messages.ReduceEvent reduceEvent)
    {
        _logger.LogDebug($"ReduceEvent received: OrderId={reduceEvent.OrderId} ReducedVolume={reduceEvent.ReducedVolume}");
        _reduceEventSubject.OnNext(reduceEvent.ToDto());
    }

    public void RejectEvent(Application.Messages.RejectEvent rejectEvent)
    {
        _logger.LogDebug($"RejectEvent received: OrderId={rejectEvent.OrderId} RejectedVolume={rejectEvent.RejectedVolume}");
        _rejectEventSubject.OnNext(rejectEvent.ToDto());
    }

    public void TradeEvent(Application.Messages.TradeEvent tradeEvent)
    {
        _logger.LogDebug($"TradeEvent received: OrderId={tradeEvent.TakerOrderId} TakeOrderCompleted={tradeEvent.TakeOrderCompleted}");
        _tradeEventSubject.OnNext(tradeEvent.ToDto());
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _commandResultSubject.Dispose();
        _orderBookSubject.Dispose();
        _reduceEventSubject.Dispose();
        _rejectEventSubject.Dispose();
        _tradeEventSubject.Dispose();
        _disposed = true;
    }
}
