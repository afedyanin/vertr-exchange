using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;
internal class ApiCommandResult : IApiCommandResult
{
    public CommandResultCode ResultCode { get; init; }

    public long OrderId { get; init; }

    public DateTime Timestamp { get; init; }

    public IEngineEvent? RootEvent { get; init; }

    public L2MarketData? MarketData { get; init; }

    public static IApiCommandResult Create(OrderCommand command, DateTime timestamp)
    {
        return new ApiCommandResult
        {
            ResultCode = command.ResultCode,
            OrderId = command.OrderId,
            RootEvent = command.EngineEvent,
            MarketData = command.MarketData,
            Timestamp = timestamp,
        };
    }
}
