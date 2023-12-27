using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.OrderManagement;

namespace Vertr.Terminal.Application.StreamEvents.Orders;
internal sealed class OrderEventRequestHandler(
    IOrderRepository orderRepository,
    ITradeEventsRepository tradeEventsRepository,
    ILogger<OrderEventRequestHandler> logger) :
    IRequestHandler<ReduceRequest>,
    IRequestHandler<RejectRequest>,
    IRequestHandler<TradeRequest>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly ILogger<OrderEventRequestHandler> _logger = logger;

    public async Task Handle(ReduceRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.ReduceEvent ?? throw new ArgumentNullException(nameof(request));

        var evt = OrderEventFactory.Create(orderEvent);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Reduce event processed. OrderId={orderId}", orderEvent.OrderId);
    }

    public async Task Handle(RejectRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.RejectEvent ?? throw new ArgumentNullException(nameof(request));

        var evt = OrderEventFactory.Create(orderEvent);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Reject event processed. OrderId={orderId}", orderEvent.OrderId);
    }

    public async Task Handle(TradeRequest request, CancellationToken cancellationToken)
    {
        var orderEvent = request.TradeEvent ?? throw new ArgumentNullException(nameof(request));

        await _tradeEventsRepository.Save(request.TradeEvent);

        await HandleTakerTrade(orderEvent);

        foreach (var makerTrade in orderEvent.Trades)
        {
            await HandleMakerTrade(orderEvent, makerTrade);
        }
    }

    private async Task HandleTakerTrade(TradeEvent tradeEvent)
    {
        var takerOrder = await _orderRepository.GetById(tradeEvent.TakerOrderId);
        var evt = OrderEventFactory.Create(tradeEvent, takerOrder);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Taker trade event processed. OrderId={orderId}", tradeEvent.TakerOrderId);
    }

    private async Task HandleMakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        var evt = OrderEventFactory.Create(tradeEvent, makerTrade);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Maker trade event processed. OrderId={orderId}", makerTrade.MakerOrderId);
    }
}
