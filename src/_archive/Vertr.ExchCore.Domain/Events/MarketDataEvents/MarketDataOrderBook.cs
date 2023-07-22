using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertr.ExchCore.Domain.Events.MarketDataEvents;

public class MarketDataOrderBook
{
    public int Symbol { get; set; }

    public IEnumerable<MarketDataOrderBookRecord> Asks { get; set; } = Enumerable.Empty<MarketDataOrderBookRecord>();

    public IEnumerable<MarketDataOrderBookRecord> Bids { get; set; } = Enumerable.Empty<MarketDataOrderBookRecord>();

    public long Timestamp { get; set; }
}
