namespace Vertr.Terminal.Domain.MarketData;

public record class MarketDataItem
{
    public DateTime TimeStamp { get; init; }

    public int SymbolId { get; init; }

    public decimal DayOpen { get; init; }

    public decimal DayLow { get; init; }

    public decimal DayHigh { get; init; }

    public decimal LastChange { get; init; }

    public decimal Change
    {
        get
        {
            return Price - DayOpen;
        }
    }

    public double PercentChange
    {
        get
        {
            return (double)Math.Round(Change / Price, 4);
        }
    }

    public decimal Price { get; init; }

    public MarketDataItem Update(DateTime timeStamp, decimal price)
    {
        if (Price == price)
        {
            return this;
        }

        var lastChange = price - Price;
        var dayOpen = DayOpen == 0 ? price : DayOpen;
        var dayLow = price < DayLow || DayLow == 0 ? price : DayLow;
        var dayHigh = price > DayHigh ? price : DayHigh;


        return new MarketDataItem
        {
            SymbolId = SymbolId,
            TimeStamp = timeStamp,
            Price = price,
            DayHigh = dayHigh,
            DayLow = dayLow,
            DayOpen = dayOpen,
            LastChange = lastChange,
        };
    }
}
