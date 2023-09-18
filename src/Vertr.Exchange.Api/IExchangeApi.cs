using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api;

public interface IExchangeApi : IDisposable
{
    void Send(IApiCommand command);

    Task<IApiCommandResult> SendAsync(IApiCommand command, CancellationToken token = default);
}
