using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;
public record class PositionPnlRecordDto
{
    public DateTime Timestamp { get; set; }
    public PositionDirection Direction { get; set; }
    public decimal PnL { get; set; }
    public decimal FixedPnL { get; set; }
    public decimal OpenVolume { get; set; }
    public decimal OpenPriceSum { get; set; }
    public bool IsEmpty { get; set; }
}
