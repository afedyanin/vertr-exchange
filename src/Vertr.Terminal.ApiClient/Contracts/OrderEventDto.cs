using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;

public record class OrderEventDto
{
    public long OrderId { get; init; }

    public DateTime TimeStamp { get; init; }

    public long Seq { get; init; }

    public OrderEventSource EventSource { get; init; } = OrderEventSource.None;

    public CommandResultCode? CommandResultCode { get; init; }

    public OrderAction? Action { get; init; }

    public bool OrderCompleted { get; init; }

    public decimal? Price { get; init; }

    public long? Volume { get; init; }
}
