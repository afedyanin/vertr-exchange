using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Shared.Reports.Dtos;
public record PositionDto
{
    public long Uid { get; set; }

    public int Symbol { get; set; }

    public PositionDirection Direction { get; set; }

    // Size
    public decimal OpenVolume { get; set; }

    public decimal PnL { get; set; }

    public decimal FixedPnL { get; set; }

    public decimal OpenPriceSum { get; set; }
}
