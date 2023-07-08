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
        _logger.LogInformation("Processing data from {Priority} {Handler}", Priority, nameof(OrderJournaling));
    }
}
