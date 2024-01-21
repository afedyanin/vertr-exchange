using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Domain.Abstractions;
public interface IMarketDataService
{
    Task ProcessTradeEvent(TradeEvent tradeEvent);
}
