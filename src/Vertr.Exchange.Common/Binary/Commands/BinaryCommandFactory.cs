using System.Text.Json;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Common.Binary.Commands;
public static class BinaryCommandFactory
{
    public static IBinaryCommand? GetBinaryCommand(BinaryDataType commandType, byte[] data)
    {
#pragma warning disable IDE0072 // Add missing cases
        return commandType switch
        {
            BinaryDataType.COMMAND_ADD_ACCOUNTS
                => JsonSerializer.Deserialize<BatchAddAccountsCommand>(data),
            BinaryDataType.COMMAND_ADD_SYMBOLS
                => JsonSerializer.Deserialize<BatchAddSymbolsCommand>(data),
            BinaryDataType.NONE => null,
            _ => null,// TODO: Handle unknown command type
        };
#pragma warning restore IDE0072 // Add missing cases
    }
}
