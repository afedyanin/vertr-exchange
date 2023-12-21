using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IEngineEvent
{
    EngineEventType EventType { get; }

    bool ActiveOrderCompleted { get; }

    bool MatchedOrderCompleted { get; }

    long MatchedOrderId { get; }

    long MatchedOrderUid { get; }

    decimal Price { get; }

    long Size { get; }

    byte[] BinaryData { get; }

    IEngineEvent? NextEvent { get; set; }
}
