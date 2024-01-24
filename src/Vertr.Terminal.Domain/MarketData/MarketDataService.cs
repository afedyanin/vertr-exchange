using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Domain.MarketData;

internal sealed class MarketDataService(IMarketDataRepository marketDataRepository) : IMarketDataService
{
    private readonly IMarketDataRepository _marketDataRepository = marketDataRepository;

    public Task ProcessTradeEvent(TradeEvent tradeEvent)
    {
        var symbolId = tradeEvent.Symbol;
        // var price = tradeEvent.Trades.Average(mt => mt.Price);
        var price = tradeEvent.Trades.Select(mt => mt.Price).Last();
        return _marketDataRepository.Update(symbolId, tradeEvent.Timestamp, price);
    }
}
