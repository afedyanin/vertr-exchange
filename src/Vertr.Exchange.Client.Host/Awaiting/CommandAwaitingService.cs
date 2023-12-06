using System.Collections.Concurrent;

namespace Vertr.Exchange.Client.Host.Awaiting;

internal sealed class CommandAwaitingService(ILogger<CommandAwaitingService> logger) : ICommandAwaitingService
{
    private readonly ConcurrentDictionary<long, TaskCompletionSource<CommandResponse>> _commands =
        new ConcurrentDictionary<long, TaskCompletionSource<CommandResponse>>();

    private readonly ConcurrentDictionary<long, CommandResponse> _missedCommands =
        new ConcurrentDictionary<long, CommandResponse>();

    private readonly ILogger<CommandAwaitingService> _logger = logger;

    public Task<CommandResponse> Register(long commandId, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<CommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Check if it already done.
        if (_missedCommands.TryRemove(commandId, out var res))
        {
            tcs.SetResult(res);
            return tcs.Task;
        }

        if (!_commands.TryAdd(commandId, tcs))
        {
            throw new InvalidOperationException($"Command with Id={commandId} is already registered.");
        }

        _logger.LogDebug("Command with Id={commandId} is registered.", commandId);

        cancellationToken.Register(UnregisterWait, new WaitContext(commandId, cancellationToken));

        return tcs.Task;
    }

    public void Complete(CommandResponse response)
    {
        var commandId = response.CommandResult.OrderId;

        if (_commands.TryRemove(commandId, out var tcs))
        {
            _logger.LogDebug("Command with Id={commandId} is completed.", commandId);
            tcs.SetResult(response);
        }
        else
        {
            _logger.LogWarning("Command with Id={commandId} is not awaited.", commandId);
            _missedCommands.TryAdd(response.CommandResult.OrderId, response);
        }
    }

    private void UnregisterWait(object? idObject)
    {
        var context = (WaitContext)idObject!;

        if (_commands.TryRemove(context.CommandId, out var tcs))
        {
            tcs.TrySetCanceled(context.CancellationToken);
        }
        else
        {
            // ???
        }
    }

    private readonly record struct WaitContext(long CommandId, CancellationToken CancellationToken);
}
