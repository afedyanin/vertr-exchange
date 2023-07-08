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
        _logger.LogInformation("Processing data from {Priority}-{Handler}", Priority, nameof(OrderEventNotificator));
    }
}
