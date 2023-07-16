using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class SampleOrderReplication : IOrderCommandSubscriber
{
    private readonly ILogger<SampleOrderReplication> _logger;

    public SampleOrderReplication(
        ILogger<SampleOrderReplication> logger)
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

