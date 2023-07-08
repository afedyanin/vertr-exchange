using Disruptor;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

internal class DisruptorOrderCommandEventHanlder : IEventHandler<OrderCommand>
{
    private readonly IOrderCommandEventHandler _eventHandler;

    public DisruptorOrderCommandEventHanlder(
        IOrderCommandEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }
    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _eventHandler.HandleEvent(data, sequence, endOfBatch);
    }
}
