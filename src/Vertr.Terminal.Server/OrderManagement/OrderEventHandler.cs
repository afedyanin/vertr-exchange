using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient.Contracts;
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
        var order = await _orderRepository.GetById(request.OrderId);

        if (order == null)
        {
            var message = $"Order with OrderId={request.OrderId} is not found.";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }

        var evt = OrderEventFactory.Create(request, result);
        order.AddEvent(evt);

        _logger.LogDebug("Move order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandleCancelOrderRequest(CancelOrderRequest request, ApiCommandResult result)
    {
        var order = await _orderRepository.GetById(request.OrderId);

        if (order == null)
        {
            var message = $"Order with OrderId={request.OrderId} is not found.";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }

        var evt = OrderEventFactory.CreateCancel(result);
        order.AddEvent(evt);

        _logger.LogDebug("Cancel order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandleReduceOrderRequest(ReduceOrderRequest request, ApiCommandResult result)
    {
        var order = await _orderRepository.GetById(request.OrderId);

        if (order == null)
        {
            var message = $"Order with OrderId={request.OrderId} is not found.";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }

        var evt = OrderEventFactory.Create(request, result);
        order.AddEvent(evt);

        _logger.LogDebug("Reduce order request processed. OrderId={orderId}", request.OrderId);
    }

    public async Task HandlePlaceOrderRequest(PlaceOrderRequest request, ApiCommandResult result)
    {
        var order = await _orderRepository.GetById(result.OrderId);

        if (order != null)
        {
            var message = $"Order with OrderId={result.OrderId} already added.";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }

        order = new Order(
            result.OrderId,
            request.UserId,
            request.Symbol,
            request.Price,
            request.Size,
            request.Action,
            request.OrderType
            );

        var evt = OrderEventFactory.Create(request, result);
        order.AddEvent(evt);

        var added = await _orderRepository.Add(order);

        _logger.LogDebug("Place order request processed. OrderId={orderId}. Saved={added}", result.OrderId, added);
    }

    public async Task HandleReduceEvent(ReduceEvent reduceEvent)
    {
        var order = await _orderRepository.GetById(reduceEvent.OrderId);

        if (order == null)
        {
            _logger.LogError("Reduce event failed. Order with OrderId={orderId} is not found.", reduceEvent.OrderId);
            return;
        }

        var evt = OrderEventFactory.Create(reduceEvent);
        order.AddEvent(evt);

        _logger.LogDebug("Reduce event processed. OrderId={orderId}", reduceEvent.OrderId);
    }

    public async Task HandleRejectEvent(RejectEvent rejectEvent)
    {
        var order = await _orderRepository.GetById(rejectEvent.OrderId);

        if (order == null)
        {
            _logger.LogError("Reject event failed. Order with OrderId={orderId} is not found.", rejectEvent.OrderId);
            return;
        }

        var evt = OrderEventFactory.Create(rejectEvent);
        order.AddEvent(evt);

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
        var order = await _orderRepository.GetById(tradeEvent.TakerOrderId);

        if (order == null)
        {
            _logger.LogError("Trade event failed. Taker order with OrderId={orderId} is not found.", tradeEvent.TakerOrderId);
            return;
        }

        var evt = OrderEventFactory.Create(tradeEvent);
        order.AddEvent(evt);

        _logger.LogDebug("Taker trade event processed. OrderId={orderId}", tradeEvent.TakerOrderId);
    }
    private async Task HandleMakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        var order = await _orderRepository.GetById(makerTrade.MakerOrderId);

        if (order == null)
        {
            _logger.LogError("Trade event failed. Maker order with OrderId={orderId} is not found.", makerTrade.MakerOrderId);
            return;
        }

        var evt = OrderEventFactory.Create(tradeEvent, makerTrade);
        order.AddEvent(evt);

        _logger.LogDebug("Maker trade event processed. OrderId={orderId}", makerTrade.MakerOrderId);
    }

    public async Task Reset()
    {
        await _orderRepository.Reset();
    }
}
