using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;

internal static class BinaryCommandStub
{
    public static OrderCommand CreateAddSymbolsCommand(int[] symbolIds)
    {
        var symbols = new List<ISymbolSpecification>(symbolIds.Length);

        foreach (int symbolId in symbolIds)
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
            Symbols = symbols.ToArray(),
        };

        return cmd.ToOrderCommand();
    }

    private sealed class SymbolSpecification : ISymbolSpecification
    {
        public int SymbolId { get; set; }

        public SymbolType Type { get; set; }

        public int Currency { get; set; }
    }
}
