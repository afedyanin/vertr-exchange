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
        _logger.LogInformation("Processing data from {Priority} {Handler}", Priority, nameof(OrderReplication));
    }
}

