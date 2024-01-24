namespace Vertr.Terminal.ApiClient.Contracts;
public record MarketDataItemDto
{
    public int SymbolId { get; init; }

    public DateTime TimeStamp { get; init; }

    public decimal DayOpen { get; init; }

    public decimal DayLow { get; init; }

    public decimal DayHigh { get; init; }

    public decimal LastChange { get; init; }

    public decimal Change { get; init; }

    public double PercentChange { get; init; }

    public decimal Price { get; init; }
}
