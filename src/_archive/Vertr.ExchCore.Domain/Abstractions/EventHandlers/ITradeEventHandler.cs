using Vertr.ExchCore.Domain.Events.TradeEvents;

namespace Vertr.ExchCore.Domain.Abstractions.EventHandlers;

public interface ITradeEventHandler
{
    void Handle(IEnumerable<TradeEventBase> tradeEvents);
}
