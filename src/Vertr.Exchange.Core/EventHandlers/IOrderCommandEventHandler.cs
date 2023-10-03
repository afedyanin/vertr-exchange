using Disruptor;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Core.EventHandlers;
public interface IOrderCommandEventHandler : IEventHandler<OrderCommand>
{
    int ProcessingStep { get; }
}
