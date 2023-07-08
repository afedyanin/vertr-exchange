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

internal class OrderMatching : IOrderCommandSubscriber
{
    private readonly ILogger<OrderMatching> _logger;

    public OrderMatching(
        ILogger<OrderMatching> logger)
    {
        _logger = logger;
    }
    public int Priority => (int)GroupPriority.Processing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _logger.LogInformation("Processing data from {Priority} {Handler}", Priority, nameof(OrderMatching));
    }
}
