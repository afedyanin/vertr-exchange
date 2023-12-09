using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Contracts;

public record CommandResult
{
    public CommandResultCode CommandResultCode { get; set; }

    public long OrderId { get; set; }

    public DateTime Timestamp { get; set; }

    public Level2MarketData? MarketData { get; set; }

    public ExchangeEvent[] Events { get; set; } = [];
}
