using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api.Commands;
public record class ReduceOrderCommand : IApiCommand
{
    public long OrderId { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }

    public int ReduceSize { get; set; }

    public void Fill(ref OrderCommand command)
    {
        throw new NotImplementedException();
    }
}
