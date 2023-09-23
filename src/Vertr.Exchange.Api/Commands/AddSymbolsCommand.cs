using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;

public class AddSymbolsCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.BINARY_DATA_COMMAND;

    private readonly SymbolSpecification[] _symbols;

    public AddSymbolsCommand(
        long orderId,
        DateTime timestamp,
        SymbolSpecification[] symbols) : base(orderId, timestamp)
    {
        _symbols = symbols ?? Array.Empty<SymbolSpecification>();
    }

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
