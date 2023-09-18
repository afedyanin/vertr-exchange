using Microsoft.Extensions.Logging;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure.EventHandlers;

public class LoggingProcessor : IOrderCommandEventHandler
{
    private readonly ILogger<LoggingProcessor> _logger;

    public int ProcessingStep => 800;

    public LoggingProcessor(ILogger<LoggingProcessor> logger)
    {
        _logger = logger;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _logger.LogInformation("Processing: OrderId={OrderId}  Sequence={Sequence}", data.OrderId, sequence);
    }
}
