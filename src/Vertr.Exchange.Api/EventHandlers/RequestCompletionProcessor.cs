using Microsoft.Extensions.Logging;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Common;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Api.EventHandlers;

internal class RequestCompletionProcessor : IOrderCommandEventHandler
{
    private readonly IRequestAwaitingService _requestAwaitingService;
    private readonly ILogger<RequestCompletionProcessor> _logger;

    public int ProcessingStep => 1000;

    public RequestCompletionProcessor(
        IRequestAwaitingService requestAwaitingService,
        ILogger<RequestCompletionProcessor> logger)
    {
        _logger = logger;
        _requestAwaitingService = requestAwaitingService;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _logger.LogInformation("ProcessingStep={ProcessingStep}. Completing order request. OrderId={OrderId}", ProcessingStep, data.OrderId);

        var response = new AwaitingResponse(data);
        _requestAwaitingService.Complete(response);
    }
}
