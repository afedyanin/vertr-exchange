using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure.EventHandlers;
internal class RequestCompletionProcessor : IOrderCommandEventHandler
{
    private readonly IRequestAwaitingService _requestAwaitingService;

    public int ProcessingStep => 10000;

    public RequestCompletionProcessor(IRequestAwaitingService requestAwaitingService)
    {
        _requestAwaitingService = requestAwaitingService;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        var response = new AwaitingResponse(data);

        _requestAwaitingService.Complete(response);
    }
}
