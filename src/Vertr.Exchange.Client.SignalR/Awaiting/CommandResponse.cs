using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Client.SignalR.Awaiting;
public record class CommandResponse
{
    public ApiCommandResult CommandResult { get; }

    public CommandResponse(ApiCommandResult commandResult)
    {
        CommandResult = commandResult;
    }
}
