using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Vertr.Exchange.Api.Awaiting;

internal class RequestAwatingService : IRequestAwaitingService
{
    private readonly ConcurrentDictionary<long, TaskCompletionSource<AwaitingResponse>> _requests;
    private readonly ILogger<RequestAwatingService> _logger;

    public RequestAwatingService(ILogger<RequestAwatingService> logger)
    {
        _logger = logger;
        _requests = new ConcurrentDictionary<long, TaskCompletionSource<AwaitingResponse>>();
    }

    public Task<AwaitingResponse> Register(long orderId, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<AwaitingResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_requests.TryAdd(orderId, tcs))
        {
            throw new InvalidOperationException($"Request with orderId={orderId} is already registered.");
        }

        _logger.LogDebug("Awating request with OrderId={OrderId} is registered.", orderId);

        cancellationToken.Register(UnregisterWait, new WaitContext(orderId, cancellationToken));

        return tcs.Task;
    }

    public void Complete(AwaitingResponse response)
    {
        var orderId = response.OrderCommand.OrderId;
        if (_requests.TryRemove(orderId, out var tcs))
        {
            _logger.LogDebug("Awating request with OrderId={OrderId} is completed.", orderId);
            tcs.SetResult(response);
        }
        else
        {
            _logger.LogError("Cannot remove awating request with OrderId={OrderId}.", orderId);
        }
    }

    public Task<long[]> GetAwatingRequests()
    {
        return Task.FromResult(_requests.Keys.ToArray());
    }

    private void UnregisterWait(object? idObject)
    {
        // Как это работает?
        var context = (WaitContext)idObject!;

        if (_requests.TryRemove(context.OrderId, out var tcs))
        {
            tcs.TrySetCanceled(context.CancellationToken);
        }
        else
        {
            // ???
        }
    }

    private readonly record struct WaitContext(long OrderId, CancellationToken CancellationToken);
}
