using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api;

public interface IExchangeApi : IDisposable
{
    Guid Id { get; }

    void Send(IApiCommand command);
}
