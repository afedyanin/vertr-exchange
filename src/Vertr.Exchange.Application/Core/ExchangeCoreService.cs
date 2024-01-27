using System.Runtime.CompilerServices;
using Disruptor;
using Disruptor.Dsl;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Application.EventHandlers;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;

[assembly: InternalsVisibleTo("Vertr.Exchange.Core.Tests")]
[assembly: InternalsVisibleTo("Vertr.Exchange.Application.Tests")]

namespace Vertr.Exchange.Application.Core;

internal class ExchangeCoreService : IExchangeCoreService
{
    private readonly Disruptor<OrderCommand> _disruptor;
    private readonly RingBuffer<OrderCommand> _ringBuffer;
    private readonly ILogger<ExchangeCoreService> _logger;

    private bool _disposed;

    public ExchangeCoreService(
        IEnumerable<IOrderCommandEventHandler> handlers,
        ILogger<ExchangeCoreService> logger)
    {
        _logger = logger;

        _disruptor = new Disruptor<OrderCommand>(() =>
            new OrderCommand(), ringBufferSize: 1024);

        ConfigureHandlers(handlers);

        _ringBuffer = _disruptor.Start();
        _logger.LogInformation("Exchange core started at {Time}. BufferSize={Size}", DateTime.Now, _ringBuffer.BufferSize);
    }

    public void Send(IApiCommand apiCommand)
    {
        using var scope = _disruptor.PublishEvent();
        var cmd = scope.Event();
        apiCommand.Fill(ref cmd);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _logger.LogInformation("Exchange core shutdown at {Time}", DateTime.Now);
        _disruptor.Shutdown();
        _disposed = true;
    }

    private void ConfigureHandlers(IEnumerable<IOrderCommandEventHandler> handlers)
    {
        EventHandlerGroup<OrderCommand>? eventHandlerGroup = null;

        var subscriberGroups = handlers.GroupBy(s => s.ProcessingStep);

        foreach (var subscriberGroup in subscriberGroups.OrderBy(g => g.Key))
        {
            var currentGroup = subscriberGroup?.ToArray();

            if (currentGroup is null)
            {
                continue;
            }

            if (eventHandlerGroup is null)
            {
                eventHandlerGroup = _disruptor.HandleEventsWith(currentGroup);
            }
            else
            {
                eventHandlerGroup = eventHandlerGroup.Then(currentGroup);
            }
        }
    }
}
