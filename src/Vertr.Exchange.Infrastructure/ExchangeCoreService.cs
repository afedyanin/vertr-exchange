using Disruptor;
using Disruptor.Dsl;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Common;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Infrastructure;

internal class ExchangeCoreService : IExchangeCoreService
{
    private readonly Disruptor<OrderCommand> _disruptor;
    private readonly RingBuffer<OrderCommand> _ringBuffer;
    private readonly IRequestAwaitingService _requestAwaitingService;
    private readonly ILogger<ExchangeCoreService> _logger;

    public ExchangeCoreService(
        IEnumerable<IOrderCommandEventHandler> handlers,
        IRequestAwaitingService requestAwaitingService,
        ILogger<ExchangeCoreService> logger)
    {
        _logger = logger;
        _requestAwaitingService = requestAwaitingService;

        _disruptor = new Disruptor<OrderCommand>(() =>
            new OrderCommand(), ringBufferSize: 1024);

        ConfigureHandlers(handlers);

        _ringBuffer = _disruptor.Start();
        _logger.LogInformation("Exchange core started at {Time}. BufferSize={Size}", DateTime.Now, _ringBuffer.BufferSize);
    }

    public async Task<OrderCommand> Process(OrderCommand orderCommand, CancellationToken token = default)
    {
        using (var scope = _disruptor.PublishEvent())
        {
            var cmd = scope.Event();

            cmd.OrderId = orderCommand.OrderId;
            cmd.Action = orderCommand.Action;
            cmd.Command = orderCommand.Command;
            cmd.Timestamp = orderCommand.Timestamp;
            cmd.ResultCode = orderCommand.ResultCode;
            cmd.BinaryCommandType = orderCommand.BinaryCommandType;
            cmd.BinaryData = orderCommand.BinaryData;
            cmd.Price = orderCommand.Price;
            cmd.Size = orderCommand.Size;
            cmd.Filled = orderCommand.Filled;
            cmd.Symbol = orderCommand.Symbol;
            cmd.OrderType = orderCommand.OrderType;
            cmd.Uid = orderCommand.Uid;
        }

        var result = await _requestAwaitingService.Register(orderCommand.OrderId, token);

        return result.OrderCommand;
    }

    public void Dispose()
    {
        _logger.LogInformation("Exchange core shutdown at {Time}", DateTime.Now);
        _disruptor.Shutdown();
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
