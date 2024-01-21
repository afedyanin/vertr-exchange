using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Application.StreamEvents.Orders;
internal sealed class OrderEventRequestHandler(
    IOrderEventService orderEventService,
    IMarketDataService marketDataService,
    IPortolioService portolioService,
    ITradeEventsRepository tradeEventsRepository,
    ILogger<OrderEventRequestHandler> logger) :
    IRequestHandler<ReduceRequest>,
    IRequestHandler<RejectRequest>,
    IRequestHandler<TradeRequest>
{
    private readonly IOrderEventService _orderEventService = orderEventService;
    private readonly IMarketDataService _marketDataService = marketDataService;
    private readonly IPortolioService _portolioService = portolioService;

    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly ILogger<OrderEventRequestHandler> _logger = logger;

    public Task Handle(ReduceRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.ReduceEvent ?? throw new ArgumentNullException(nameof(request));
        _logger.LogDebug("Processing Reduce event. OrderId={orderId}", orderEvent.OrderId);

        return _orderEventService.ProcessReduceEvent(orderEvent);
    }

    public Task Handle(RejectRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.RejectEvent ?? throw new ArgumentNullException(nameof(request));
        _logger.LogDebug("Processing Reject event. OrderId={orderId}", orderEvent.OrderId);

        return _orderEventService.ProcessRejectEvent(orderEvent);
    }

    public Task Handle(TradeRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.TradeEvent ?? throw new ArgumentNullException(nameof(request));
        _logger.LogDebug("Processing Trade event. Taker OrderId={orderId}", orderEvent.TakerOrderId);

        return Task.WhenAll(
            _tradeEventsRepository.Save(orderEvent),
            _orderEventService.ProcessTradeEvent(orderEvent),
            _marketDataService.ProcessTradeEvent(orderEvent),
            _portolioService.ProcessTradeEvent(orderEvent)
            );
    }
}
