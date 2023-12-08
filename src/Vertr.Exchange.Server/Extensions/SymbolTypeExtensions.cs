using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class SymbolTypeExtensions
{
    public static Common.Enums.SymbolType ToDomain(this SymbolType sType)
    {
        return sType switch
        {
            SymbolType.CURRENCY_EXCHANGE_PAIR => Common.Enums.SymbolType.CURRENCY_EXCHANGE_PAIR,
            SymbolType.FUTURES_CONTRACT => Common.Enums.SymbolType.FUTURES_CONTRACT,
            SymbolType.OPTION => Common.Enums.SymbolType.OPTION,
            SymbolType.EQUITY => Common.Enums.SymbolType.EQUITY,
            _ => throw new InvalidOperationException($"Unknown symbol type: {sType}"),
        };
    }
}
