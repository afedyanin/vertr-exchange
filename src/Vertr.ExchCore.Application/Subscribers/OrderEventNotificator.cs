using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class OrderEventNotificator : IOrderCommandSubscriber
{
    private readonly ILogger<OrderEventNotificator> _logger;

    public OrderEventNotificator(
        ILogger<OrderEventNotificator> logger)
    {
        _logger = logger;
    }
    public int Priority => (int)GroupPriority.PostProcessing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        var nextId = 301L;

        _logger.LogInformation("Priority={Priority} OrderId={OrderId} will be set to {NextId}",
            Priority,
            data.OrderId,
            nextId);

        data.OrderId = nextId;
    }
}
