using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary.Commands;
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

    public static OrderCommand ToOrderCommand(this BatchAddSymbolsCommand cmd)
    {
        var orderCommand = new OrderCommand()
        {
            Command = OrderCommandType.BINARY_DATA_COMMAND,
            BinaryCommandType = BinaryDataType.COMMAND_ADD_SYMBOLS,
            BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd)),
        };

        return orderCommand;
    }

    public static OrderCommand ToOrderCommand(this BatchAddAccountsCommand cmd)
    {
        var orderCommand = new OrderCommand()
        {
            Command = OrderCommandType.BINARY_DATA_COMMAND,
            BinaryCommandType = BinaryDataType.COMMAND_ADD_ACCOUNTS,
            BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd)),
        };

        return orderCommand;
    }
}
