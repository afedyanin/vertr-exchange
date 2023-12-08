using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class CommandResultCodeExtensions
{
    public static Contracts.Enums.CommandResultCode ToDto(this CommandResultCode resultCode)
    {
        return resultCode switch
        {
            CommandResultCode.DROP => Contracts.Enums.CommandResultCode.DROP,
            CommandResultCode.NEW => Contracts.Enums.CommandResultCode.NEW,
            CommandResultCode.VALID_FOR_MATCHING_ENGINE => Contracts.Enums.CommandResultCode.VALID_FOR_MATCHING_ENGINE,
            CommandResultCode.SUCCESS => Contracts.Enums.CommandResultCode.SUCCESS,
            CommandResultCode.ACCEPTED => Contracts.Enums.CommandResultCode.ACCEPTED,
            CommandResultCode.AUTH_INVALID_USER => Contracts.Enums.CommandResultCode.AUTH_INVALID_USER,
            CommandResultCode.AUTH_TOKEN_EXPIRED => Contracts.Enums.CommandResultCode.AUTH_TOKEN_EXPIRED,
            CommandResultCode.INVALID_SYMBOL => Contracts.Enums.CommandResultCode.INVALID_SYMBOL,
            CommandResultCode.INVALID_PRICE_STEP => Contracts.Enums.CommandResultCode.INVALID_PRICE_STEP,
            CommandResultCode.UNSUPPORTED_SYMBOL_TYPE => Contracts.Enums.CommandResultCode.UNSUPPORTED_SYMBOL_TYPE,
            CommandResultCode.RISK_NSF => Contracts.Enums.CommandResultCode.RISK_NSF,
            CommandResultCode.RISK_INVALID_RESERVE_BID_PRICE => Contracts.Enums.CommandResultCode.RISK_INVALID_RESERVE_BID_PRICE,
            CommandResultCode.RISK_ASK_PRICE_LOWER_THAN_FEE => Contracts.Enums.CommandResultCode.RISK_ASK_PRICE_LOWER_THAN_FEE,
            CommandResultCode.RISK_MARGIN_TRADING_DISABLED => Contracts.Enums.CommandResultCode.RISK_MARGIN_TRADING_DISABLED,
            CommandResultCode.MATCHING_UNKNOWN_ORDER_ID => Contracts.Enums.CommandResultCode.MATCHING_UNKNOWN_ORDER_ID,
            CommandResultCode.MATCHING_DUPLICATE_ORDER_ID => Contracts.Enums.CommandResultCode.MATCHING_DUPLICATE_ORDER_ID,
            CommandResultCode.MATCHING_UNSUPPORTED_COMMAND => Contracts.Enums.CommandResultCode.MATCHING_UNSUPPORTED_COMMAND,
            CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID => Contracts.Enums.CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID,
            CommandResultCode.MATCHING_ORDER_BOOK_ALREADY_EXISTS => Contracts.Enums.CommandResultCode.MATCHING_ORDER_BOOK_ALREADY_EXISTS,
            CommandResultCode.MATCHING_UNSUPPORTED_ORDER_TYPE => Contracts.Enums.CommandResultCode.MATCHING_UNSUPPORTED_ORDER_TYPE,
            CommandResultCode.MATCHING_MOVE_REJECTED_DIFFERENT_PRICE => Contracts.Enums.CommandResultCode.MATCHING_MOVE_REJECTED_DIFFERENT_PRICE,
            CommandResultCode.MATCHING_MOVE_FAILED_PRICE_OVER_RISK_LIMIT => Contracts.Enums.CommandResultCode.MATCHING_MOVE_FAILED_PRICE_OVER_RISK_LIMIT,
            CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE => Contracts.Enums.CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE,
            CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS => Contracts.Enums.CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ZERO => Contracts.Enums.CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ZERO,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_SAME => Contracts.Enums.CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_SAME,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_MANY => Contracts.Enums.CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_MANY,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF => Contracts.Enums.CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF,
            CommandResultCode.USER_MGMT_NON_ZERO_ACCOUNT_BALANCE => Contracts.Enums.CommandResultCode.USER_MGMT_NON_ZERO_ACCOUNT_BALANCE,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS => Contracts.Enums.CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS => Contracts.Enums.CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED => Contracts.Enums.CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED,
            CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED => Contracts.Enums.CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED,
            CommandResultCode.USER_MGMT_USER_NOT_FOUND => Contracts.Enums.CommandResultCode.USER_MGMT_USER_NOT_FOUND,
            CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS => Contracts.Enums.CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS,
            CommandResultCode.BINARY_COMMAND_FAILED => Contracts.Enums.CommandResultCode.BINARY_COMMAND_FAILED,
            CommandResultCode.REPORT_QUERY_UNKNOWN_TYPE => Contracts.Enums.CommandResultCode.REPORT_QUERY_UNKNOWN_TYPE,
            CommandResultCode.STATE_PERSIST_RISK_ENGINE_FAILED => Contracts.Enums.CommandResultCode.STATE_PERSIST_RISK_ENGINE_FAILED,
            CommandResultCode.STATE_PERSIST_MATCHING_ENGINE_FAILED => Contracts.Enums.CommandResultCode.STATE_PERSIST_MATCHING_ENGINE_FAILED,
            CommandResultCode.RISK_GENERIC_ERROR => Contracts.Enums.CommandResultCode.RISK_GENERIC_ERROR,
            CommandResultCode.MATCHING_GENERIC_ERROR => Contracts.Enums.CommandResultCode.MATCHING_GENERIC_ERROR,
            _ => throw new InvalidOperationException($"Unknown CommandResultCode={resultCode}"),
        };
    }
}
