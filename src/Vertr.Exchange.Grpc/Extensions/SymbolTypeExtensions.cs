namespace Vertr.Exchange.Grpc.Extensions;

internal static class SymbolTypeExtensions
{
    public static Common.Enums.SymbolType ToDomain(this SymbolType sType)
    {
        return sType switch
        {
            SymbolType.CurrencyExchangePair => Common.Enums.SymbolType.CURRENCY_EXCHANGE_PAIR,
            SymbolType.FuturesContract => Common.Enums.SymbolType.FUTURES_CONTRACT,
            SymbolType.Option => Common.Enums.SymbolType.OPTION,
            SymbolType.Equity => Common.Enums.SymbolType.EQUITY,
            _ => Common.Enums.SymbolType.CURRENCY_EXCHANGE_PAIR,
        };
    }
}
