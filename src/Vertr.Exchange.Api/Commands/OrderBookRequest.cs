using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api.Commands;
public record class OrderBookRequest : IApiCommand
{
    public int Symbol { get; set; }

    public int Size { get; set; }

    public void Fill(ref OrderCommand command)
    {
        throw new NotImplementedException();
    }
}
