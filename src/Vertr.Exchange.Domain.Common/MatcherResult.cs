using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Common;

public sealed class MatcherResult(long filled, IEnumerable<IEngineEvent>? tradeEvents = null)
{
    public long Filled { get; } = filled;

    public IEnumerable<IEngineEvent> TradeEvents { get; } = tradeEvents ?? Array.Empty<IEngineEvent>();
}
