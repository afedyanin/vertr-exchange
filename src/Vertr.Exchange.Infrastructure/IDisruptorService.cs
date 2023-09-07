using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure;

internal interface IDisruptorService : IDisposable
{
    void Send(OrderCommand orderCommand);
}
