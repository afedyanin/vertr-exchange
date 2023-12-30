using System.Runtime.CompilerServices;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Core;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi(IExchangeCoreService exchangeCoreService) : IExchangeApi
{
    private readonly IExchangeCoreService _exchangeCoreService = exchangeCoreService;

    public Guid Id { get; } = Guid.NewGuid();

    public void Send(IApiCommand command)
    {
        _exchangeCoreService.Send(command);
    }

    public void Dispose()
    {
        _exchangeCoreService?.Dispose();
    }
}
