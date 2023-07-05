namespace Vertr.Exchange.Contracts;

public class OrderCommand
{
    public long Uuid { get; set; }

    public long OrderId { get; set; }

    public long Timestamp { get; set; }

    public OrderCommandType CommandType { get; set; }

    public OrderType OrderType { get; set; }

    public OrderAction OrderAction { get; set; }

    public int Symbol { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }
}
