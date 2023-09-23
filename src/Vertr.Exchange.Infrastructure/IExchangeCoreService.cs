using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Infrastructure;

public interface IExchangeCoreService : IDisposable
{
    void Send(IApiCommand apiCommand);
}
