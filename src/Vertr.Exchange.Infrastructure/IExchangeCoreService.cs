using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure;

public interface IExchangeCoreService : IDisposable
{
    void Send(OrderCommand orderCommand);
}
