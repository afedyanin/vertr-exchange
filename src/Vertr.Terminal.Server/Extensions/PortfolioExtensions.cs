using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class PortfolioExtensions
{
    public static PortfolioDto[] ToDto(this Portfolio[] portfolios)
        => portfolios.Select(ToDto).ToArray();

    public static PortfolioDto ToDto(this Portfolio portfolio)
        => new PortfolioDto
        {
            Uid = portfolio.Uid,
            Positions = portfolio.Positions.ToDto(),
        };
}
