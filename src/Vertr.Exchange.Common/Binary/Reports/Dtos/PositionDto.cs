using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary.Reports.Dtos;
public class PositionDto
{
    public long Uid { get; set; }

    public int Symbol { get; set; }

    public PositionDirection Direction { get; set; }

    // Size
    public decimal OpenVolume { get; set; }

    // Realized PnL
    public decimal RealizedPnL { get; set; }
}