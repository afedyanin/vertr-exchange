using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Domain.PortfolioManagement;
internal sealed class PortolioService(IPortfolioRepository portfolioRepository) : IPortolioService
{
    private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;

    public async Task ProcessTradeEvent(TradeEvent tradeEvent)
    {
        var trades = PositionTradeFactory.Create(tradeEvent);

        // TODO: Optimize it
        foreach (var trade in trades)
        {
            await ProcessTrade(trade);
        }
    }

    public async Task ProcessTrade(PositionTrade positionTrade)
    {
        var portfolio = await _portfolioRepository.GetByUid(positionTrade.Uid);
        portfolio ??= new Portfolio(positionTrade.Uid);

        portfolio.ApplyTrade(positionTrade);
        await _portfolioRepository.AddOrUpdate(portfolio);
    }
}
