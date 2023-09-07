using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vertr.Exchange.Infrastructure.Tests")]

namespace Vertr.Exchange.Infrastructure;

internal class RequestAwatingService : IRequestAwaitingService
{
    private readonly ConcurrentDictionary<long, TaskCompletionSource<AwaitingResponse>> _requests;

    public RequestAwatingService()
    {
        _requests = new ConcurrentDictionary<long, TaskCompletionSource<AwaitingResponse>>();
    }

    public Task<AwaitingResponse> Register(long orderId, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<AwaitingResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_requests.TryAdd(orderId, tcs))
        {
            throw new InvalidOperationException($"Request with orderId={orderId} is already registered.");
        }

        cancellationToken.Register(UnregisterWait, new WaitContext(orderId, cancellationToken));

        return tcs.Task;
    }

    public void Complete(AwaitingResponse response)
    {
        if (_requests.TryRemove(response.OrderCommand.OrderId, out var tcs))
        {
            tcs.SetResult(response);
        }
        else
        {
            // ???
        }
    }

    public Task<long[]> GetAwatingRequests()
    {
        return Task.FromResult(_requests.Keys.ToArray());
    }

    private void UnregisterWait(object? idObject)
    {
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
