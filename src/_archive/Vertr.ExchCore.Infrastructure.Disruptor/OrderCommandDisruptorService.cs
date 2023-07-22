using System.Collections.Generic;
using System.Net.NetworkInformation;
using Disruptor;
using Disruptor.Dsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;
using Vertr.ExchCore.Infrastructure.Disruptor.Configuration;
using Vertr.ExchCore.Infrastructure.Disruptor.Extensions;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

internal class OrderCommandDisruptorService : IOrderCommandPublisher, IDisposable
{
    private readonly Disruptor<OrderCommand> _disruptor;
    private readonly DisruptorOptions _config;
    private readonly ILogger<OrderCommandDisruptorService> _logger;

    public OrderCommandDisruptorService(
        IOptions<DisruptorOptions> config,
        IEnumerable<IOrderCommandSubscriber> subscribers,
        ILogger<OrderCommandDisruptorService> logger)
    {
        _logger = logger;
        _config = config.Value;

        _disruptor = new Disruptor<OrderCommand>(() =>
            new OrderCommand(), ringBufferSize: _config.RingBufferSize);
        _disruptor.AttachEventHandlers(subscribers);

        _disruptor.Start();
        _logger.LogInformation("Disruptor started at {Time}", DateTime.Now);
    }

    public void Publish(OrderCommand command)
    {
        using var scope = _disruptor.PublishEvent();

        var cmd = scope.Event();

        // TODO: Fill order command
        cmd.OrderId = 100L;
        // data.Message = ping;
    }

    public void Dispose()
    {
        _logger.LogInformation("Disruptor shutdown at {Time}", DateTime.Now);
        _disruptor.Shutdown();
    }
}
