using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Infrastructure.EventHandlers;
internal class MatchingEngineProcessor : IOrderCommandEventHandler
{
    private readonly IOrderMatchingEngine _orderMatchingEngine;

    public int ProcessingStep => 200;

    public MatchingEngineProcessor(IOrderMatchingEngine orderMatchingEngine)
    {
        _orderMatchingEngine = orderMatchingEngine;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _orderMatchingEngine.ProcessOrder(sequence, data);
    }
}
