namespace Vertr.Terminal.ApiClient.Contracts;
public record class PortfolioDto
{
    public long Uid { get; set; }

    public PositionDto[] Positions { get; set; } = [];

}
