using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Api.Extensions;

public static class CommandResultCodeExtensions
{
    public static ResultCode ToGrpc(this CommandResultCode resultCode)
    {
        return resultCode switch
        {
            CommandResultCode.DROP => ResultCode.Drop,
            CommandResultCode.NEW => ResultCode.New,
            CommandResultCode.VALID_FOR_MATCHING_ENGINE => ResultCode.ValidForMatchingEngine,
            CommandResultCode.SUCCESS => ResultCode.Success,
            CommandResultCode.ACCEPTED => ResultCode.Accepted,
            CommandResultCode.AUTH_INVALID_USER => ResultCode.AuthInvalidUser,
            CommandResultCode.AUTH_TOKEN_EXPIRED => ResultCode.AuthTokenExpired,
            CommandResultCode.INVALID_SYMBOL => ResultCode.InvalidSymbol,
            CommandResultCode.INVALID_PRICE_STEP => ResultCode.InvalidPriceStep,
            CommandResultCode.UNSUPPORTED_SYMBOL_TYPE => ResultCode.UnsupportedSymbolType,
            CommandResultCode.RISK_NSF => ResultCode.RiskNsf,
            CommandResultCode.RISK_INVALID_RESERVE_BID_PRICE => ResultCode.RiskInvalidReserveBidPrice,
            CommandResultCode.RISK_ASK_PRICE_LOWER_THAN_FEE => ResultCode.RiskAskPriceLowerThanFee,
            CommandResultCode.RISK_MARGIN_TRADING_DISABLED => ResultCode.RiskMarginTradingDisabled,
            CommandResultCode.MATCHING_UNKNOWN_ORDER_ID => ResultCode.MatchingUnknownOrderId,
            CommandResultCode.MATCHING_DUPLICATE_ORDER_ID => ResultCode.MatchingDuplicateOrderId,
            CommandResultCode.MATCHING_UNSUPPORTED_COMMAND => ResultCode.MatchingUnsupportedCommand,
            CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID => ResultCode.MatchingInvalidOrderBookId,
            CommandResultCode.MATCHING_ORDER_BOOK_ALREADY_EXISTS => ResultCode.MatchingOrderBookAlreadyExists,
            CommandResultCode.MATCHING_UNSUPPORTED_ORDER_TYPE => ResultCode.MatchingUnsupportedOrderType,
            CommandResultCode.MATCHING_MOVE_REJECTED_DIFFERENT_PRICE => ResultCode.MatchingMoveRejectedDifferentPrice,
            CommandResultCode.MATCHING_MOVE_FAILED_PRICE_OVER_RISK_LIMIT => ResultCode.MatchingMoveFailedPriceOverRiskLimit,
            CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE => ResultCode.MatchingReduceFailedWrongSize,
            CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS => ResultCode.UserMgmtUserAlreadyExists,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ZERO => ResultCode.UserMgmtAccountBalanceAdjustmentZero,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_SAME => ResultCode.UserMgmtAccountBalanceAdjustmentAlreadyAppliedSame,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_MANY => ResultCode.UserMgmtAccountBalanceAdjustmentAlreadyAppliedMany,
            CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF => ResultCode.UserMgmtAccountBalanceAdjustmentNsf,
            CommandResultCode.USER_MGMT_NON_ZERO_ACCOUNT_BALANCE => ResultCode.UserMgmtNonZeroAccountBalance,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS => ResultCode.UserMgmtUserNotSuspendableHasPositions,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS => ResultCode.UserMgmtUserNotSuspendableNonEmptyAccounts,
            CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED => ResultCode.UserMgmtUserNotSuspended,
            CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED => ResultCode.UserMgmtUserAlreadySuspended,
            CommandResultCode.USER_MGMT_USER_NOT_FOUND => ResultCode.UserMgmtUserNotFound,
            CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS => ResultCode.SymbolMgmtSymbolAlreadyExists,
            CommandResultCode.BINARY_COMMAND_FAILED => ResultCode.BinaryCommandFailed,
            CommandResultCode.REPORT_QUERY_UNKNOWN_TYPE => ResultCode.ReportQueryUnknownType,
            CommandResultCode.STATE_PERSIST_RISK_ENGINE_FAILED => ResultCode.StatePersistRiskEngineFailed,
            CommandResultCode.STATE_PERSIST_MATCHING_ENGINE_FAILED => ResultCode.StatePersistMatchingEngineFailed,
            _ => ResultCode.New,
        };
    }
}
