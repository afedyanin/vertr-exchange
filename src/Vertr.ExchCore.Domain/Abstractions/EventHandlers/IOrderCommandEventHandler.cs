using Vertr.ExchCore.Domain.Events.OrderEvents;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions.EventHandlers;

public interface IOrderCommandEventHandler
{
    void Handle(OrderCommandEvent commandEvent);
}
