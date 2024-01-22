namespace Vertr.Terminal.ApiClient.Contracts;
public record class PositionDto
{
    public long Uid { get; set; }

    public int Symbol { get; set; }

    public PositionTradeDto[] Trades { get; set; } = [];

    public PositionPnlRecordDto[] PnlHistory { get; set; } = [];

    public PositionPnlRecordDto? Pnl { get; set; }

}
