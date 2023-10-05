using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Core.EventHandlers;

internal class RiskEnginePostProcessor : IOrderCommandEventHandler
{
    private readonly IOrderRiskEngine _orderRiskEngine;

    public int ProcessingStep => 300;

    public RiskEnginePostProcessor(IOrderRiskEngine orderRiskEngine)
    {
        _orderRiskEngine = orderRiskEngine;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _orderRiskEngine.PostProcessCommand(sequence, data);
    }
}
