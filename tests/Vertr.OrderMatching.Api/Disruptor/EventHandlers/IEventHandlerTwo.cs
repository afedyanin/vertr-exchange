using Disruptor;
using Vertr.OrderMatching.Api.Disruptor.Events;

namespace Vertr.OrderMatching.Api.Disruptor.EventHandlers
{
    public interface IEventHandlerTwo : IEventHandler<PingEvent>
    {
    }
}