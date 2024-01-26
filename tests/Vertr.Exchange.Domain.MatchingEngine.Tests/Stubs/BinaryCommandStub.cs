using System.Text;
using System.Text.Json;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Binary.Commands;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.MatchingEngine.Tests.Stubs;

internal static class BinaryCommandStub
{
    public static OrderCommand CreateAddSymbolsCommand(int[] symbolIds)
    {
        var symbols = new List<SymbolSpecification>(symbolIds.Length);

        foreach (var symbolId in symbolIds)
        {
            var spec = new SymbolSpecification
            {
                SymbolId = symbolId,
                Type = SymbolType.CURRENCY_EXCHANGE_PAIR,
                Currency = 10,
            };

            symbols.Add(spec);
        }

        var cmd = new BatchAddSymbolsCommand()
        {
            Symbols = [.. symbols],
        };

        return ToOrderCommand(cmd);
    }

    private static OrderCommand ToOrderCommand(BatchAddSymbolsCommand cmd)
    {
        var orderCommand = new OrderCommand()
        {
            Command = OrderCommandType.BINARY_DATA_COMMAND,
            BinaryCommandType = BinaryDataType.COMMAND_ADD_SYMBOLS,
            BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd)),
        };

        return orderCommand;
    }
}
