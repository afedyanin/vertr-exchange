using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Binary.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.Domain.MatchingEngine.Binary;
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
