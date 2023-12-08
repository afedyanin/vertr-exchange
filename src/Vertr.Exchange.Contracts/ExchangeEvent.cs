using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Contracts;

public record ExchangeEvent
{
    public EngineEventType EventType { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public bool MatchedOrderCompleted { get; set; }

    public long MatchedOrderId { get; set; }

    public long MatchedOrderUid { get; set; }

    public decimal Price { get; set; }

    public long Size { get; set; }

    public byte[] BinaryData { get; set; } = [];
}
