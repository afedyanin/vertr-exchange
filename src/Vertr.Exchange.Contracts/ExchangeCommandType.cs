namespace Vertr.Exchange.Contracts;

public enum ExchangeCommandType : byte
{
    NONE = 0,
    ADD_USER = 10,
    BALANCE_ADJUSTMENT = 11,
    SUSPEND_USER = 12,
    RESUME_USER = 13,
    BINARY_DATA_QUERY = 90,
    BINARY_DATA_COMMAND = 91,
    PERSIST_STATE_MATCHING = 110,
    PERSIST_STATE_RISK = 111,
    GROUPING_CONTROL = 118,
    NOP = 120,
    RESET = 124,
    SHUTDOWN_SIGNAL = 127,
}
