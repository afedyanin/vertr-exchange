namespace Vertr.Exchange.Api.Commands;
public record class MoveOrderCommand : ApiCommand
{
    public int OrderId { get; set; }

    public decimal NewPrice { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }
}
