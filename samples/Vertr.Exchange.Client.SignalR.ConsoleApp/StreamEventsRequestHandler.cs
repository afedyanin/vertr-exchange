using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Client.SignalR.Requests;

namespace Vertr.Exchange.Client.SignalR.ConsoleApp;

internal sealed class StreamEventsRequestHandler(ILogger<StreamEventsRequestHandler> logger) :
    IRequestHandler<HandleOrderBookRequest>,
    IRequestHandler<HandleReduceRequest>,
    IRequestHandler<HandleRejectRequest>,
    IRequestHandler<HandleTradeRequest>
{
    private readonly ILogger<StreamEventsRequestHandler> _logger = logger;

    public Task Handle(HandleOrderBookRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Order book received: {orderBook}", request.OrderBook);

        return Task.CompletedTask;
    }

    public Task Handle(HandleReduceRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Reduce event received: {reduceEvent}", request.ReduceEvent);

        return Task.CompletedTask;
    }

    public Task Handle(HandleRejectRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Reject event received: {rejectEvent}", request.RejectEvent);

        return Task.CompletedTask;
    }

    public Task Handle(HandleTradeRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trade event received: {tradeEvent}", request.TradeEvent);

        return Task.CompletedTask;
    }
}
