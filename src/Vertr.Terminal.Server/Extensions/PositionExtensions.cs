using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class PositionExtensions
{
    public static PositionDto[] ToDto(this Position[] positions)
        => positions.Select(ToDto).ToArray();

    public static PositionDto ToDto(this Position position)
        => new PositionDto
        {
            Uid = position.Uid,
            Symbol = position.Symbol,
            Trades = position.Trades.ToDto(),
            Pnl = position.Pnl.ToDto(),
            PnlHistory = position.PnlHistory.ToDto(),
        };
}
