using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class BynaryDataTypeExtensions
{
    public static Common.Enums.BinaryDataType ToDomain(this BinaryDataType dataType)
    {
        return dataType switch
        {
            BinaryDataType.NONE => Common.Enums.BinaryDataType.NONE,
            BinaryDataType.COMMAND_ADD_ACCOUNTS => Common.Enums.BinaryDataType.COMMAND_ADD_ACCOUNTS,
            BinaryDataType.COMMAND_ADD_SYMBOLS => Common.Enums.BinaryDataType.COMMAND_ADD_SYMBOLS,
            BinaryDataType.QUERY_STATE_HASH => Common.Enums.BinaryDataType.QUERY_STATE_HASH,
            BinaryDataType.QUERY_SINGLE_USER_REPORT => Common.Enums.BinaryDataType.QUERY_SINGLE_USER_REPORT,
            BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE => Common.Enums.BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE,
            _ => throw new InvalidOperationException($"Unknown bynary data type: {dataType}"),
        };
    }

    public static BinaryDataType ToDto(this Common.Enums.BinaryDataType dataType)
    {
        return dataType switch
        {
            Common.Enums.BinaryDataType.NONE => BinaryDataType.NONE,
            Common.Enums.BinaryDataType.COMMAND_ADD_ACCOUNTS => BinaryDataType.COMMAND_ADD_ACCOUNTS,
            Common.Enums.BinaryDataType.COMMAND_ADD_SYMBOLS => BinaryDataType.COMMAND_ADD_SYMBOLS,
            Common.Enums.BinaryDataType.QUERY_STATE_HASH => BinaryDataType.QUERY_STATE_HASH,
            Common.Enums.BinaryDataType.QUERY_SINGLE_USER_REPORT => BinaryDataType.QUERY_SINGLE_USER_REPORT,
            Common.Enums.BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE => BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE,
            _ => throw new InvalidOperationException($"Unknown bynary data type: {dataType}"),
        };
    }
}
