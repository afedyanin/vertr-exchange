using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Core;

public interface IExchangeCoreService : IDisposable
{
    void Send(IApiCommand apiCommand);
}
