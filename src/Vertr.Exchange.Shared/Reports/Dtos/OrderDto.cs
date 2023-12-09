using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Shared.Reports.Dtos;
public class OrderDto
{
    public OrderAction Action { get; set; }

    public long OrderId { get; set; }

    public decimal Price { get; set; }

    public long Size { get; set; }

    public long Filled { get; set; }

    public long Uid { get; set; }

    public DateTime Timestamp { get; set; }

    public long Remaining { get; set; }

    public bool Completed { get; set; }
}
