using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.OrderManagement;

public class OrderEventHandler(
    IOrdersRepository orderRepository,
    ILogger<OrderEventHandler> logger) : IOrderEventHandler
{
    private readonly IOrdersRepository _orderRepository = orderRepository;
    private readonly ILogger<OrderEventHandler> _logger = logger;

    public async Task HandleMoveOrderRequest(MoveOrderRequest request, ApiCommandResult result)
    {
        var evt = OrderEventFactory.Create(request, result);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Move order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandleCancelOrderRequest(CancelOrderRequest request, ApiCommandResult result)
    {
        var evt = OrderEventFactory.CreateCancel(result);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Cancel order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandleReduceOrderRequest(ReduceOrderRequest request, ApiCommandResult result)
    {
        var evt = OrderEventFactory.Create(request, result);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Reduce order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandlePlaceOrderRequest(PlaceOrderRequest request, ApiCommandResult result)
    {
        var order = new Order(
            result.OrderId,
            request.UserId,
            request.Symbol,
            request.Price,
            request.Size,
            request.Action,
            request.OrderType
            );

        await _orderRepository.AddOrder(order);

        var evt = OrderEventFactory.Create(request, result);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Place order request processed. OrderId={orderId}", result.OrderId);
    }

    public async Task HandleReduceEvent(ReduceEvent reduceEvent)
    {
        var evt = OrderEventFactory.Create(reduceEvent);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Reduce event processed. OrderId={orderId}", reduceEvent.OrderId);
    }

    public async Task HandleRejectEvent(RejectEvent rejectEvent)
    {
        var evt = OrderEventFactory.Create(rejectEvent);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Reject event processed. OrderId={orderId}", rejectEvent.OrderId);
    }

    public async Task HandleTradeEvent(TradeEvent tradeEvent)
    {
        await HandleTakerTrade(tradeEvent);

        foreach (var makerTrade in tradeEvent.Trades)
        {
            await HandleMakerTrade(tradeEvent, makerTrade);
        }
    }

    private async Task HandleTakerTrade(TradeEvent tradeEvent)
    {
        var evt = OrderEventFactory.Create(tradeEvent);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Taker trade event processed. OrderId={orderId}", tradeEvent.TakerOrderId);
    }

    private async Task HandleMakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        var evt = OrderEventFactory.Create(tradeEvent, makerTrade);
        await _orderRepository.AddEvent(evt);
        _logger.LogDebug("Maker trade event processed. OrderId={orderId}", makerTrade.MakerOrderId);
    }

    public async Task Reset()
    {
        await _orderRepository.Reset();
    }
}
