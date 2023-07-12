using Vertr.ExchCore.Domain.Entities;
using Vertr.ExchCore.Domain.Events.MarketDataEvents;

namespace Vertr.ExchCore.Domain.Abstractions.EventHandlers;

public interface IMarketDataEventHandler
{
    void Handle(OrderBook orderBook);
}
