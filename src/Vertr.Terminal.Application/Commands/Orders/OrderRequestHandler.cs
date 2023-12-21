using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.OrderManagement;

namespace Vertr.Terminal.Application.Commands.Orders;

internal sealed class OrderRequestHandler(
    IOrderRepository orderRepository,
    IExchangeApiClient exchangeApiClient,
    ILogger<OrderRequestHandler> logger) :
    IRequestHandler<PlaceRequest, ApiCommandResult>,
    IRequestHandler<CancelRequest, ApiCommandResult>,
    IRequestHandler<MoveRequest, ApiCommandResult>,
    IRequestHandler<ReduceRequest, ApiCommandResult>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger<OrderRequestHandler> _logger = logger;
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;

    public async Task<ApiCommandResult> Handle(PlaceRequest request, CancellationToken cancellationToken)
    {
        var orderRequest = request.PlaceOrderRequest ?? throw new ArgumentNullException(nameof(request));

        var res = await _exchangeApiClient.PlaceOrder(orderRequest);

        var order = new Order(
            res.OrderId,
            orderRequest.UserId,
            orderRequest.Symbol,
            orderRequest.Price,
            orderRequest.Size,
            orderRequest.Action,
            orderRequest.OrderType
            );

        await _orderRepository.AddOrder(order);

        var evt = OrderEventFactory.Create(orderRequest, res);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Place order request processed. OrderId={orderId}", res.OrderId);

        return res;
    }

    public async Task<ApiCommandResult> Handle(CancelRequest request, CancellationToken cancellationToken)
    {
        var orderRequest = request.CancelOrderRequest ?? throw new ArgumentNullException(nameof(request));

        var res = await _exchangeApiClient.CancelOrder(orderRequest);

        var evt = OrderEventFactory.CreateCancel(res);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Cancel order request processed. OrderId={orderId}", orderRequest.OrderId);

        return res;
    }

    public async Task<ApiCommandResult> Handle(MoveRequest request, CancellationToken cancellationToken)
    {
        var orderRequest = request.MoveOrderRequest ?? throw new ArgumentNullException(nameof(request));

        var res = await _exchangeApiClient.MoveOrder(orderRequest);

        var evt = OrderEventFactory.Create(orderRequest, res);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Move order request processed. OrderId={orderId}", orderRequest.OrderId);

        return res;
    }

    public async Task<ApiCommandResult> Handle(ReduceRequest request, CancellationToken cancellationToken)
    {
        var orderRequest = request.ReduceOrderRequest ?? throw new ArgumentNullException(nameof(request));

        var res = await _exchangeApiClient.ReduceOrder(orderRequest);

        var evt = OrderEventFactory.Create(orderRequest, res);
        await _orderRepository.AddEvent(evt);

        _logger.LogDebug("Reduce order request processed. OrderId={orderId}", orderRequest.OrderId);

        return res;
    }
}
