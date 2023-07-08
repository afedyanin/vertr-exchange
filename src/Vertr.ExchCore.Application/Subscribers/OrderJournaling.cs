using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class OrderJournaling : IOrderCommandSubscriber
{
    private readonly ILogger<OrderEventNotificator> _logger;

    public OrderJournaling(
        ILogger<OrderEventNotificator> logger)
    {
        _logger = logger;
    }
    public int Priority => (int)GroupPriority.PreProcessing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        var nextId = 101L;

        _logger.LogInformation("Priority={Priority} OrderId={OrderId} will be set to {NextId}",
            Priority,
            data.OrderId,
            nextId);

        data.OrderId = nextId;
    }
}
