using Vertr.Exchange.Common.Messages;

namespace Vertr.Terminal.ExchangeClient.Awaiting;
public record class CommandResponse
{
    public ApiCommandResult CommandResult { get; }

    public CommandResponse(ApiCommandResult commandResult)
    {
        CommandResult = commandResult;
    }
}
