using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Application.StreamEvents;

internal sealed class OrderBookRequestHandler(
    IOrderBookSnapshotsRepository orderBookRepository,
    ILogger<OrderBookRequestHandler> logger) : IRequestHandler<OrderBookRequest>
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ILogger<OrderBookRequestHandler> _logger = logger;

    public async Task Handle(OrderBookRequest request, CancellationToken cancellationToken)
    {
        if (request.OrderBook == null)
        {
            return;
        }

        await _orderBookRepository.Save(request.OrderBook);
        _logger.LogDebug("Order Book received: {orderBook}", request.OrderBook);
    }
}
