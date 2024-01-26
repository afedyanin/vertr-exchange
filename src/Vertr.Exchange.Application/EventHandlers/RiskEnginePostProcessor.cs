using Microsoft.Extensions.Logging;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.EventHandlers;

internal class RiskEnginePostProcessor(
    IOrderRiskEngine orderRiskEngine,
    ILogger<RiskEnginePostProcessor> logger) : IOrderCommandEventHandler
{
    private readonly IOrderRiskEngine _orderRiskEngine = orderRiskEngine;
    private readonly ILogger<RiskEnginePostProcessor> _logger = logger;

    public int ProcessingStep => 300;

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            _orderRiskEngine.PostProcessCommand(sequence, data);
        }
        catch (Exception ex)
        {
            data.ResultCode = CommandResultCode.RISK_GENERIC_ERROR;
            _logger.LogError(ex, "Error processing command OrderId={OrderId} Message={Message}", data.OrderId, ex.Message);
        }
    }
}
