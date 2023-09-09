namespace Vertr.Exchange.Api.Commands;
public record class CancelOrderCommand : ApiCommand
{
    public long OrderId { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }
}
