using System.Reactive.Subjects;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Server.Extensions;

namespace Vertr.Exchange.Server.MessageHandlers;

public class ObservableMessageHandler : IObservableMessageHandler, IMessageHandler, IDisposable
{
    private readonly Subject<ApiCommandResult> _commandResultSubject = new Subject<ApiCommandResult>();
    private readonly Subject<OrderBook> _orderBookSubject = new Subject<OrderBook>();
    private readonly Subject<ReduceEvent> _reduceEventSubject = new Subject<ReduceEvent>();
    private readonly Subject<RejectEvent> _rejectEventSubject = new Subject<RejectEvent>();
    private readonly Subject<TradeEvent> _tradeEventSubject = new Subject<TradeEvent>();

    private bool _disposed;
    private readonly Guid _identity;

    private readonly ILogger<ObservableMessageHandler> _logger;

    public ObservableMessageHandler(ILogger<ObservableMessageHandler> logger)
    {
        _logger = logger;
        _identity = Guid.NewGuid();
        _logger.LogInformation("Starting ObservableMessageHandler. Identity={id}", _identity);
    }

    public IObservable<ApiCommandResult> ApiCommandResultStream() => _commandResultSubject;

    public IObservable<OrderBook> OrderBookStream() => _orderBookSubject;

    public IObservable<ReduceEvent> ReduceEventStream() => _reduceEventSubject;

    public IObservable<RejectEvent> RejectEventStream() => _rejectEventSubject;

    public IObservable<TradeEvent> TradeEventStream() => _tradeEventSubject;

    public void CommandResult(Common.Messages.ApiCommandResult apiCommandResult)
    {
        _logger.LogInformation($"CommandResult received: OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
        _commandResultSubject.OnNext(apiCommandResult.ToDto());
    }

    public void OrderBook(Common.Messages.OrderBook orderBook)
    {
        _logger.LogInformation($"OrderBook received: Symbol={orderBook.Symbol}");
        _orderBookSubject.OnNext(orderBook.ToDto());
    }

    public void ReduceEvent(Common.Messages.ReduceEvent reduceEvent)
    {
        _logger.LogInformation($"ReduceEvent received: OrderId={reduceEvent.OrderId} ReducedVolume={reduceEvent.ReducedVolume}");
        _reduceEventSubject.OnNext(reduceEvent.ToDto());
    }

    public void RejectEvent(Common.Messages.RejectEvent rejectEvent)
    {
        _logger.LogInformation($"RejectEvent received: OrderId={rejectEvent.OrderId} RejectedVolume={rejectEvent.RejectedVolume}");
        _rejectEventSubject.OnNext(rejectEvent.ToDto());
    }

    public void TradeEvent(Common.Messages.TradeEvent tradeEvent)
    {
        _logger.LogInformation($"TradeEvent received: OrderId={tradeEvent.TakerOrderId} TakeOrderCompleted={tradeEvent.TakeOrderCompleted}");
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
