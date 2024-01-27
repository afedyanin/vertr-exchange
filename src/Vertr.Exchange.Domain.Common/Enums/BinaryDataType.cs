namespace Vertr.Exchange.Domain.Common.Enums;

public enum BinaryDataType
{
    NONE = 0,
    COMMAND_ADD_ACCOUNTS = 1002,
    COMMAND_ADD_SYMBOLS = 1003,
    QUERY_STATE_HASH = 10001,
    QUERY_SINGLE_USER_REPORT = 10002,
    QUERY_TOTAL_CURRENCY_BALANCE = 10003,
}
