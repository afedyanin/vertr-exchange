using Microsoft.Extensions.Logging;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.EventHandlers;
internal class MatchingEngineProcessor(
    IOrderMatchingEngine orderMatchingEngine,
    ILogger<MatchingEngineProcessor> logger) : IOrderCommandEventHandler
{
    private readonly IOrderMatchingEngine _orderMatchingEngine = orderMatchingEngine;
    private readonly ILogger<MatchingEngineProcessor> _logger = logger;

    public int ProcessingStep => 200;

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            _orderMatchingEngine.ProcessOrder(sequence, data);
        }
        catch (Exception ex)
        {
            data.ResultCode = CommandResultCode.MATCHING_GENERIC_ERROR;
            _logger.LogError(ex, "Error processing command OrderId={OrderId} Message={Message}", data.OrderId, ex.Message);
        }
    }
}
