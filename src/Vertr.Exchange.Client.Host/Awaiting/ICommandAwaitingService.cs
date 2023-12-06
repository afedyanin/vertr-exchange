namespace Vertr.Exchange.Client.Host.Awaiting;

public interface ICommandAwaitingService
{
    Task<CommandResponse> Register(long commandId, CancellationToken cancellationToken = default);

    void Complete(CommandResponse response);
}
