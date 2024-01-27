using Microsoft.Extensions.Logging;
using Vertr.Exchange.Domain.Common;

namespace Vertr.Exchange.Application.EventHandlers;

internal class LoggingProcessor(ILogger<LoggingProcessor> logger) : IOrderCommandEventHandler
{
    private readonly ILogger<LoggingProcessor> _logger = logger;

    public int ProcessingStep => 800;

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _logger.LogDebug("Processing: OrderId={OrderId}  CommandType={CommandType}", data.OrderId, data.Command);

        /*
        if (data.Command == Shared.Enums.OrderCommandType.BINARY_DATA_QUERY
            && data.BinaryCommandType == Shared.Enums.BinaryDataType.QUERY_SINGLE_USER_REPORT
            && data.EngineEvent != null)
        {
            var evt = data.EngineEvent;
            if (evt != null && evt.EventType == Shared.Enums.EngineEventType.BINARY_EVENT)
            {
                var report = JsonSerializer.Deserialize<SingleUserReportResult>(evt.BinaryData);
                _logger.LogWarning("REPORT Received: {report}", report);
            }
        }
        */
    }
}
