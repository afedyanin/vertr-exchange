using System.Text.Json;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Binary;
internal sealed class BinaryCommandProcessor
{
    private readonly Action<OrderCommand, BinaryCommand>? _binarycommandHandler;

    public BinaryCommandProcessor(Action<OrderCommand, BinaryCommand> binarycommandHandler)
    {
        _binarycommandHandler = binarycommandHandler;
    }

    public CommandResultCode AcceptBinaryCommand(OrderCommand cmd)
    {
        if (cmd.Command is OrderCommandType.BINARY_DATA_COMMAND)
        {
            var command = RestoreCommand(cmd.BinaryCommandType, cmd.BinaryData);

            if (command != null)
            {
                _binarycommandHandler?.Invoke(cmd, command);
            }
        }

        return CommandResultCode.SUCCESS;
    }

    private BinaryCommand? RestoreCommand(BinaryDataType commandType, byte[] data)
    {
        return commandType switch
        {
            BinaryDataType.COMMAND_ADD_ACCOUNTS
                => JsonSerializer.Deserialize<BatchAddAccountsCommand>(data),
            BinaryDataType.COMMAND_ADD_SYMBOLS
                => JsonSerializer.Deserialize<BatchAddSymbolsCommand>(data),
            BinaryDataType.NONE => null,
            _ => null,// TODO: Handle unknown command type
        };
    }
}
