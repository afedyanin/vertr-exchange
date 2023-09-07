using Disruptor;
using Disruptor.Dsl;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure;

internal class DisruptorService : IDisruptorService
{
    private readonly Disruptor<OrderCommand> _disruptor;
    private readonly RingBuffer<OrderCommand> _ringBuffer;
    private readonly ILogger<DisruptorService> _logger;

    public DisruptorService(
        Action<Disruptor<OrderCommand>> configure,
        ILogger<DisruptorService> logger)
    {
        _logger = logger;

        _disruptor = new Disruptor<OrderCommand>(() =>
            new OrderCommand(), ringBufferSize: 1024);

        configure(_disruptor);

        _ringBuffer = _disruptor.Start();
        _logger.LogInformation("Disruptor started at {Time}. BufferSize={Size}", DateTime.Now, _ringBuffer.BufferSize);
    }

    public void Send(OrderCommand orderCommand)
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
