using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class OrderReplication : IOrderCommandSubscriber
{
    private readonly ILogger<OrderReplication> _logger;

    public OrderReplication(
        ILogger<OrderReplication> logger)
    {
        _logger = logger;
    }
    public int Priority => (int)GroupPriority.PreProcessing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        var nextId = 102L;

        _logger.LogInformation("Priority={Priority} OrderId={OrderId} will be set to {NextId}",
            Priority,
            data.OrderId,
            nextId);

        data.OrderId = nextId;
    }
}

