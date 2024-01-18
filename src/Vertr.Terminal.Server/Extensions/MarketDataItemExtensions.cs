using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.MarketData;

namespace Vertr.Terminal.Server.Extensions;

internal static class MarketDataItemExtensions
{
    public static MarketDataItemDto[] ToDto(this MarketDataItem[] items)
        => items.Select(ToDto).ToArray();

    public static MarketDataItemDto ToDto(this MarketDataItem item)
        => new MarketDataItemDto
        {
            Change = item.Change,
            DayHigh = item.DayHigh,
            DayLow = item.DayLow,
            DayOpen = item.DayOpen,
            LastChange = item.LastChange,
            PercentChange = item.PercentChange,
            Price = item.Price,
            SymbolId = item.SymbolId,
        };
}
