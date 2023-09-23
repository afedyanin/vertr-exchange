using Vertr.Exchange.Common;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Api.EventHandlers;
internal class SimpleEventsProcessor : IOrderCommandEventHandler
{
    public int ProcessingStep => 1010;

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            SendCommandResult(data, sequence);
            SendTradeEvents(data);
            SendMarketData(data);
        }
        catch
        {
        }
    }

    private void SendCommandResult(OrderCommand data, long sequence)
    {

    }

    private void SendTradeEvents(OrderCommand data)
    {

    }

    private void SendMarketData(OrderCommand data)
    {

    }
}
