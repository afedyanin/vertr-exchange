namespace Vertr.ExchCore.Domain.Enums;

public enum CommandResultCode
{
    NEW = 0,
    VALID_FOR_MATCHING_ENGINE = 1,

    SUCCESS = 100,
    ACCEPTED = 110,

    AUTH_INVALID_USER = -1001,
    AUTH_TOKEN_EXPIRED = -1002,

    INVALID_SYMBOL = -1201,
    INVALID_PRICE_STEP = -1202,
    UNSUPPORTED_SYMBOL_TYPE = -1203,

    RISK_NSF = -2001,
    RISK_INVALID_RESERVE_BID_PRICE = -2002,
    RISK_ASK_PRICE_LOWER_THAN_FEE = -2003,
    RISK_MARGIN_TRADING_DISABLED = -2004,

    MATCHING_UNKNOWN_ORDER_ID = -3002,
    // MATCHING_DUPLICATE_ORDER_ID = -3003,
    MATCHING_UNSUPPORTED_COMMAND = -3004,
    MATCHING_INVALID_ORDER_BOOK_ID = -3005,
    // MATCHING_ORDER_BOOK_ALREADY_EXISTS = -3006,
    // MATCHING_UNSUPPORTED_ORDER_TYPE = -3007,
    // MATCHING_MOVE_REJECTED_DIFFERENT_PRICE = -3040,
    MATCHING_MOVE_FAILED_PRICE_OVER_RISK_LIMIT = -3041,
    MATCHING_REDUCE_FAILED_WRONG_SIZE = -3051,

    USER_MGMT_USER_ALREADY_EXISTS = -4001,

    // USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ZERO = -4100,
    USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_SAME = -4101,
    USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_MANY = -4102,
    USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF = -4103,
    USER_MGMT_NON_ZERO_ACCOUNT_BALANCE = -4104,

    USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS = -4130,
    USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS = -4131,
    USER_MGMT_USER_NOT_SUSPENDED = -4132,
    USER_MGMT_USER_ALREADY_SUSPENDED = -4133,

    USER_MGMT_USER_NOT_FOUND = -4201,

    SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS = -5001,

    BINARY_COMMAND_FAILED = -8001,
    REPORT_QUERY_UNKNOWN_TYPE = -8003,
    STATE_PERSIST_RISK_ENGINE_FAILED = -8010,
    STATE_PERSIST_MATCHING_ENGINE_FAILED = -8020,

    DROP = -9999,
}
