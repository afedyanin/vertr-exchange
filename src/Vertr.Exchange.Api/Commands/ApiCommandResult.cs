using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
internal class ApiCommandResult : IApiCommandResult
{
    public CommandResultCode ResultCode { get; init; }

    public long OrderId { get; init; }

    public DateTime Timestamp { get; init; }

    public IEngineEvent? RootEvent { get; init; }

    public static IApiCommandResult Create(OrderCommand command)
    {
        return new ApiCommandResult
        {
            ResultCode = command.ResultCode,
            OrderId = command.OrderId,
            Timestamp = command.Timestamp,
            RootEvent = command.EngineEvent,
        };
    }
}
