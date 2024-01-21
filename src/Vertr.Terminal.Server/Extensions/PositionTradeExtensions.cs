using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class PositionTradeExtensions
{
    public static PositionTradeDto[] ToDto(this PositionTrade[] trades)
        => trades.Select(ToDto).ToArray();

    public static PositionTradeDto ToDto(this PositionTrade positionTrade)
        => new PositionTradeDto
        {
            OrderId = positionTrade.OrderId,
            Direction = positionTrade.Direction,
            Price = positionTrade.Price,
            Seq = positionTrade.Seq,
            Symbol = positionTrade.Symbol,
            Timestamp = positionTrade.Timestamp,
            Uid = positionTrade.Uid,
            Volume = positionTrade.Volume,
        };
}
