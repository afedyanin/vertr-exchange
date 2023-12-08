using Microsoft.Extensions.Logging;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Core.EventHandlers;
internal class MatchingEngineProcessor : IOrderCommandEventHandler
{
    private readonly IOrderMatchingEngine _orderMatchingEngine;
    private readonly ILogger<MatchingEngineProcessor> _logger;

    public int ProcessingStep => 200;

    public MatchingEngineProcessor(
        IOrderMatchingEngine orderMatchingEngine,
        ILogger<MatchingEngineProcessor> logger)
    {
        _orderMatchingEngine = orderMatchingEngine;
        _logger = logger;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            _orderMatchingEngine.ProcessOrder(sequence, data);
        }
        catch (Exception ex)
        {
            data.ResultCode = Common.Enums.CommandResultCode.MATCHING_GENERIC_ERROR;
            _logger.LogError(ex, "Error processing command OrderId={OrderId} Message={Message}", data.OrderId, ex.Message);
        }
    }
}
