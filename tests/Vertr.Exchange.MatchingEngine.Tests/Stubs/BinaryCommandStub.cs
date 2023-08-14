using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary;
using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;

internal static class BinaryCommandStub
{
    public static OrderCommand CreateAddSymbolsCommand(int[] symbolIds)
    {
        var symbols = new List<CoreSymbolSpecification>(symbolIds.Length);

        foreach (int symbolId in symbolIds)
        {
            var spec = new CoreSymbolSpecification
            {
                SymbolId = symbolId,
                Type = Common.Enums.SymbolType.CURRENCY_EXCHANGE_PAIR
            };

            symbols.Add(spec);
        }

        var cmd = new BatchAddSymbolsCommand()
        {
            Symbols = symbols.ToArray(),
        };

        return cmd.ToOrderCommand();
    }
}
