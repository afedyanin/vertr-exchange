namespace Vertr.ExchCore.Domain.Events.MarketDataEvents;

public class MarketDataOrderBookRecord
{
    public long Price { get; set; }

    public long Volume { get; set; }

    public long Orders { get; set; }
}
