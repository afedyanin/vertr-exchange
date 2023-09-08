using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure;

public interface IExchangeCoreService : IDisposable
{
    Task<OrderCommand> Process(OrderCommand orderCommand, CancellationToken token = default);
}
