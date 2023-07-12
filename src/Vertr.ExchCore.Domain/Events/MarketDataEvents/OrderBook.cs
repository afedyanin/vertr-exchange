using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertr.ExchCore.Domain.Events.MarketDataEvents;

public class OrderBook
{
    public int Symbol { get; set; }

    public IEnumerable<OrderBookRecord> Asks { get; set; } = Enumerable.Empty<OrderBookRecord>();

    public IEnumerable<OrderBookRecord> Bids { get; set; } = Enumerable.Empty<OrderBookRecord>();

    public long Timestamp { get; set; }
}
