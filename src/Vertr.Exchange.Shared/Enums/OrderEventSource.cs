namespace Vertr.Exchange.Shared.Enums;

public enum OrderEventSource
{
    None = 0,
    PlaceOrderRequest = 1,
    RejectEvent = 2,
    ReduceEvent = 3,
    TakerTradeEvent = 4,
    MakerTradeEvent = 5,
    MoveOrderRequest = 6,
    ReduceOrderRequest = 7,
    CacnelOrderRequest = 8,
}
