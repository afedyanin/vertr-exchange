using Disruptor;
using Vertr.Exchange.Domain.Common;

namespace Vertr.Exchange.Application.EventHandlers;

public interface IOrderCommandEventHandler : IEventHandler<OrderCommand>
{
    int ProcessingStep { get; }
}
