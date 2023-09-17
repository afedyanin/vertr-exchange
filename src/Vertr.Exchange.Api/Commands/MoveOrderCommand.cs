using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api.Commands;
public record class MoveOrderCommand : IApiCommand
{
    public int OrderId { get; set; }

    public decimal NewPrice { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }

    public void Fill(ref OrderCommand command)
    {
        throw new NotImplementedException();
    }
}
