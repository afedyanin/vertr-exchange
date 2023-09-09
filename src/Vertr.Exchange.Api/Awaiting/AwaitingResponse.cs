using Vertr.Exchange.Common;

namespace Vertr.Exchange.Api.Awaiting;
public record class AwaitingResponse
{
    public OrderCommand OrderCommand { get; }

    public AwaitingResponse(OrderCommand orderCommand)
    {
        OrderCommand = orderCommand;
    }
}
