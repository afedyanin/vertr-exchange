using System.Collections.Generic;
using System.Net.NetworkInformation;
using Disruptor;
using Disruptor.Dsl;
using Microsoft.Extensions.Options;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;
using Vertr.ExchCore.Infrastructure.Disruptor.Abstractions;
using Vertr.ExchCore.Infrastructure.Disruptor.Configuration;
using Vertr.ExchCore.Infrastructure.Disruptor.Extensions;
using Vertr.ExchCore.Infrastructure.Disruptor.Helpers;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

internal class OrderCommandDisruptorService : IDisruptorService<OrderCommand>, IDisposable
{
    private readonly Disruptor<OrderCommand> _disruptor;
    private readonly DisruptorConfiguration _config;

    public OrderCommandDisruptorService(
        IOptions<DisruptorConfiguration> config,
        IEnumerable<IOrderCommandEventHandler[]> eventHandlers)
    {
        _config = config.Value;
        _disruptor = new Disruptor<OrderCommand>(() =>
            new OrderCommand(), ringBufferSize: _config.RingBufferSize);
        _disruptor.AttachEventHandlers(eventHandlers);
        _disruptor.Start();
    }

    public void Publish(OrderCommand command)
    {
        using var scope = _disruptor.PublishEvent();

        var cmd = scope.Event();

        // TODO: Fill order command
        // data.Id = 42;
        // data.Message = ping;
    }

    public void Dispose()
    {
        _disruptor.Shutdown();
    }
}
