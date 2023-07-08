using Disruptor;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

internal class DisruptorOrderCommandEventHanlder : IEventHandler<OrderCommand>
{
    private readonly IOrderCommandSubscriber _subscriber;

    public DisruptorOrderCommandEventHanlder(
        IOrderCommandSubscriber subscriber)
    {
        _subscriber = subscriber;
    }
    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _subscriber.HandleEvent(data, sequence, endOfBatch);
    }
}
