using Microsoft.Extensions.Logging;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Core.EventHandlers;
internal class RiskEnginePreProcessor : IOrderCommandEventHandler
{
    private readonly IOrderRiskEngine _orderRiskEngine;
    private readonly ILogger<RiskEnginePreProcessor> _logger;

    public int ProcessingStep => 100;

    public RiskEnginePreProcessor(
        IOrderRiskEngine orderRiskEngine,
        ILogger<RiskEnginePreProcessor> logger)
    {
        _orderRiskEngine = orderRiskEngine;
        _logger = logger;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            _orderRiskEngine.PreProcessCommand(sequence, data);
        }
        catch (Exception ex)
        {
            data.ResultCode = Common.Enums.CommandResultCode.RISK_GENERIC_ERROR;
            _logger.LogError(ex, "Error processing command OrderId={OrderId} Message={Message}", data.OrderId, ex.Message);
        }
    }
}
