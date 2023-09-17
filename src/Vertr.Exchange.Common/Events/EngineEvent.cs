using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Events;

public sealed class EngineEvent : IEngineEvent
{
    public EngineEventType EventType { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public bool MatchedOrderCompleted { get; set; }

    public long MatchedOrderId { get; set; }

    public long MatchedOrderUid { get; set; }

    public decimal Price { get; set; }

    public long Size { get; set; }

    public IEngineEvent? NextEvent { get; set; }

    public byte[] BinaryData { get; set; } = Array.Empty<byte>();
}