using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.MatchingEngine.Binary;
internal static class CommandExtensions
{
    public static CommandResultCode HandleCommand(
        this BatchAddSymbolsCommand cmd,
        IOrderBookProvider orderBookProvider)
    {
        foreach (var sym in cmd.Symbols)
        {
            orderBookProvider.AddSymbol(sym.SymbolId);
        }

        return CommandResultCode.SUCCESS;
    }
}
