using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions.EventHandlers;

public interface IOrderCommandResultHandler
{
    void HandleCommandResult(OrderCommandResult commandResult);
}
