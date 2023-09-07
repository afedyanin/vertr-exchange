using Disruptor;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure.EventHandlers;
public interface IOrderCommandEventHandler : IEventHandler<OrderCommand>
{
    int ProcessingStep { get; }
}
