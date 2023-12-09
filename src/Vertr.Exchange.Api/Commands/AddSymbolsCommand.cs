using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;

public class AddSymbolsCommand(
    long orderId,
    DateTime timestamp,
    SymbolSpecification[] symbols)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.BINARY_DATA_COMMAND;

    private readonly SymbolSpecification[] _symbols = symbols ?? [];

    public override void Fill(ref OrderCommand command)
    {
        var cmd = new BatchAddSymbolsCommand()
        {
            Symbols = _symbols,
        };

        base.Fill(ref command);
        command.BinaryCommandType = BinaryDataType.COMMAND_ADD_SYMBOLS;
        command.BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd));
    }
}
