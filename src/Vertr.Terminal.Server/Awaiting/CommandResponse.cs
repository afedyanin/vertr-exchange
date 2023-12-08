using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Server.Awaiting;
public record class CommandResponse
{
    public ApiCommandResult CommandResult { get; }

    public CommandResponse(ApiCommandResult commandResult)
    {
        CommandResult = commandResult;
    }
}
