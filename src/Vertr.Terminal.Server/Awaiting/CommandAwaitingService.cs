namespace Vertr.Terminal.Server.Awaiting;

internal sealed class CommandAwaitingService(ILogger<CommandAwaitingService> logger) : ICommandAwaitingService
{
    private readonly Dictionary<long, TaskCompletionSource<CommandResponse>> _commands = [];

    private readonly ILogger<CommandAwaitingService> _logger = logger;

    private static readonly object _lock = new object();

    public Task<CommandResponse> Register(long commandId, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            // Check if it is already done.
            if (_commands.Remove(commandId, out var tcs))
            {
                _logger.LogDebug("Command Id={commandId} already completed.", commandId);
                return tcs.Task;
            }

            tcs = new TaskCompletionSource<CommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!_commands.TryAdd(commandId, tcs))
            {
                throw new InvalidOperationException($"Command Id={commandId} is already registered.");
            }

            _logger.LogDebug("Command Id={commandId} is registered.", commandId);

            cancellationToken.Register(UnregisterWait, new WaitContext(commandId, cancellationToken));

            return tcs.Task;
        }
    }

    public void Complete(CommandResponse response)
    {
        lock (_lock)
        {
            var commandId = response.CommandResult.OrderId;

            if (_commands.Remove(commandId, out var tcs))
            {
                _logger.LogDebug("Command Id={commandId} is completed.", commandId);
                tcs.SetResult(response);
                return;
            }

            _logger.LogWarning("Command Id={commandId} completed before awating.", commandId);
            tcs = new TaskCompletionSource<CommandResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            tcs.SetResult(response);

            if (!_commands.TryAdd(response.CommandResult.OrderId, tcs))
            {
                throw new InvalidOperationException($"Command Id={commandId} is already registered.");
            }
        }
    }

    private void UnregisterWait(object? idObject)
    {
        var context = (WaitContext)idObject!;

        if (_commands.Remove(context.CommandId, out var tcs))
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
