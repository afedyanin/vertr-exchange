using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Core.EventHandlers;
internal class RiskEnginePreProcessor : IOrderCommandEventHandler
{
    private readonly IOrderRiskEngine _orderRiskEngine;

    public int ProcessingStep => 100;

    public RiskEnginePreProcessor(IOrderRiskEngine orderRiskEngine)
    {
        _orderRiskEngine = orderRiskEngine;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _orderRiskEngine.PreProcessCommand(sequence, data);
    }
}
