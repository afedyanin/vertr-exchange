using System.Text.Json;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary;
public static class BinaryCommandFactory
{
    public static IBinaryCommand? CreateCommand(BinaryDataType commandType, byte[] data)
    {
        return commandType switch
        {
            BinaryDataType.COMMAND_ADD_ACCOUNTS
                => JsonSerializer.Deserialize<BatchAddAccountsCommand>(data),
            BinaryDataType.COMMAND_ADD_SYMBOLS
                => JsonSerializer.Deserialize<BatchAddSymbolsCommand>(data),
            BinaryDataType.NONE => null,
            BinaryDataType.QUERY_STATE_HASH => throw new NotImplementedException(),
            BinaryDataType.QUERY_SINGLE_USER_REPORT => throw new NotImplementedException(),
            BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE => throw new NotImplementedException(),
            _ => null,// TODO: Handle unknown command type
        };
    }
}
