namespace Vertr.Exchange.Api.Commands;
public record class ReduceOrderCommand : ApiCommand
{
    public long OrderId { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }

    public int ReduceSize { get; set; }

}
