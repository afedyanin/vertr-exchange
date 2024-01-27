using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IApiCommandResult
{
    CommandResultCode ResultCode { get; }

    public long OrderId { get; }

    public DateTime Timestamp { get; }

    public IEngineEvent? RootEvent { get; }

    public L2MarketData? MarketData { get; }
}
