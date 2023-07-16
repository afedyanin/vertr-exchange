namespace Vertr.ExchCore.Domain.Enums;

public enum MatcherEventType
{
    // Trade event
    // Can be triggered by place ORDER or for MOVE order command.
    TRADE = 0,

    // Reject event
    // Can happen only when MARKET order has to be rejected by Matcher Engine due lack of liquidity
    // That basically means no ASK (or BID) orders left in the order book for any price.
    // Before being rejected active order can be partially filled.
    REJECT = 1,

    // After cancel/reduce order - risk engine should unlock deposit accordingly
    REDUCE = 2,

    // Custom binary data attached
    BINARY_EVENT = 3
}
