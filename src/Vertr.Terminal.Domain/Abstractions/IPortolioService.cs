using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Domain.Abstractions;

public interface IPortolioService
{
    Task ProcessTradeEvent(TradeEvent tradeEvent);
}
